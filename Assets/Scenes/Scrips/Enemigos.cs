using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public float velocidad = 3f;
    private Vector3[] direcciones = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
    private int indiceDireccion = 0;
    private float tiempoCambio = 2f;
    private float tiempoTranscurrido = 0f;
    public GameObject personatge;
    public float hp = 1f;
    bool isalive = true;
    public float timeWaitUntilDestoy = 0f;

    GameObject questContainer;

    // Camino fijo (Waypoints)
    public Transform[] waypoints;  // Asignar en el Inspector
    private int waypointIndex = 0;

    // Variables A*
    private List<Vector2> path = new List<Vector2>();
    private int currentPathIndex = 0;
    public LayerMask obstacleLayer;

    // Enumeración para alternar entre métodos de actualización
    private enum UpdateMethod
    {
        Original,
        AStar,
        FixedPath // Nueva opción para camino fijo
    }

    private UpdateMethod updateMethod = UpdateMethod.Original;

    void Start()
    {
        questContainer = GameObject.Find("QuestContainer");
        CambiarDireccionAleatoria();
    }

    void Update()
    {
        Debug.Log(hp);

        if (hp <= 0)
        {
            DieAnim();
        }
        else
        {
            // Alternar método manualmente
            if (Input.GetKeyDown(KeyCode.T)) updateMethod = UpdateMethod.Original;
            if (Input.GetKeyDown(KeyCode.Y)) updateMethod = UpdateMethod.AStar;
            if (Input.GetKeyDown(KeyCode.U)) updateMethod = UpdateMethod.FixedPath;

            // Ejecuta el método de movimiento seleccionado
            switch (updateMethod)
            {
                case UpdateMethod.Original:
                    movimentEnemic();
                    break;
                case UpdateMethod.AStar:
                    movimentEnemicAEstrella();
                    break;
                case UpdateMethod.FixedPath:
                    movimentEnemicCaminoFijo();
                    break;
            }
        }
    }

    private void CheckUnitHp()
    {
        if (hp <= 0f)
        {
            isalive = true;
        }
    }

    private void MovimentAleatori()
    {
        transform.Translate(direcciones[indiceDireccion] * velocidad * Time.deltaTime);
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoCambio)
        {
            tiempoTranscurrido = 0f;
            CambiarDireccionAleatoria();
        }
    }

    private void CambiarDireccionAleatoria()
    {
        indiceDireccion = Random.Range(0, direcciones.Length);
    }

    private void movimentEnemic()
    {
        Vector2 direccioEnemic = (personatge.transform.position - transform.position).normalized;
        Vector2 novaPosEnemic = new Vector2(transform.position.x + direccioEnemic.x * velocidad * Time.deltaTime, transform.position.y + direccioEnemic.y * velocidad * Time.deltaTime);
        transform.position = novaPosEnemic;
    }

    private void movimentEnemicAEstrella()
    {
        if (personatge == null)
        {
            Debug.LogWarning("La variable 'personatge' no está asignada. Asigna el jugador en el Inspector.");
            return;
        }

        if (path == null || path.Count == 0 || HaCambiadoPosicionJugador())
        {
            CalcularCaminoAEstrella();
        }

        SeguirCamino();
    }

    private bool HaCambiadoPosicionJugador()
    {
        if (path != null && path.Count > 0)
        {
            return Vector2.Distance(path[path.Count - 1], personatge.transform.position) > 0.5f;
        }
        return true;
    }

    private void CalcularCaminoAEstrella()
    {
        path.Clear();
        Vector2 start = transform.position;
        Vector2 goal = personatge.transform.position;

        List<Vector2> openSet = new List<Vector2> { start };
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Dictionary<Vector2, float> gScores = new Dictionary<Vector2, float> { { start, 0 } };
        Dictionary<Vector2, float> fScores = new Dictionary<Vector2, float> { { start, Heuristica(start, goal) } };

        while (openSet.Count > 0)
        {
            Vector2 current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScores[openSet[i]] < fScores[current])
                {
                    current = openSet[i];
                }
            }

            if (current == goal)
            {
                ReconstruirCamino(cameFrom, current);
                return;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 neighbor = current + new Vector2(i * 0.5f, j * 0.5f);

                    if (HayObstaculo(current, neighbor))
                        continue;

                    if (closedSet.Contains(neighbor))
                        continue;

                    float tentativeGScore = gScores[current] + Vector2.Distance(current, neighbor);

                    if (!gScores.ContainsKey(neighbor) || tentativeGScore < gScores[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScores[neighbor] = tentativeGScore;
                        fScores[neighbor] = tentativeGScore + Heuristica(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }

        path = null;
    }

    private float Heuristica(Vector2 from, Vector2 to)
    {
        return Vector2.Distance(from, to);
    }

    private bool HayObstaculo(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleLayer);
        return hit.collider != null;
    }

    private void ReconstruirCamino(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        path = new List<Vector2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        currentPathIndex = 0;
    }

    private void SeguirCamino()
    {
        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector2 targetPosition = path[currentPathIndex];
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 newPosition = (Vector2)transform.position + direction * velocidad * Time.deltaTime;

            transform.position = newPosition;

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }

    // ✅ Método nuevo: movimiento por camino fijo
    private void movimentEnemicCaminoFijo()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[waypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * velocidad * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0; // Repetir el camino
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void DieAnim()
    {
        //questContainer.GetComponent<ShowQuest>().IncrementCounter(1);
        Destroy(gameObject);
    }
}

