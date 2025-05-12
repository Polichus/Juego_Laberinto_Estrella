using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public float velocidad = 3f;  // Velocidad de movimiento del enemigo
    private Vector3[] direcciones = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };  // Direcciones posibles
    private int indiceDireccion = 0;  // Índice de la dirección actual
    private float tiempoCambio = 2f;  // Tiempo antes de cambiar de dirección
    private float tiempoTranscurrido = 0f;
    public GameObject personatge;
    public float hp = 1f;
    bool isalive = true;
    public float timeWaitUntilDestoy = 0f;

    GameObject questContainer;

    private Rigidbody2D rb;  // Rigidbody2D para el movimiento físico del enemigo

    // Start is called before the first frame update
    void Start()
    {
        questContainer = GameObject.Find("QuestContainer");
        rb = GetComponent<Rigidbody2D>();  // Obtiene el Rigidbody2D
        CambiarDireccionAleatoria();  // Inicializa el movimiento aleatorio
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            DieAnim();
        }
        else
        {
            // Opcionalmente, puedes mantener uno por defecto y alternar con la tecla "T":
            if (Input.GetKeyDown(KeyCode.T))
            {
                // Alterna entre los dos métodos
                if (updateMethod == UpdateMethod.Original)
                {
                    updateMethod = UpdateMethod.AStar;
                }
                else
                {
                    updateMethod = UpdateMethod.Original;
                }
            }

            // Ejecuta el método seleccionado
            if (updateMethod == UpdateMethod.Original)
            {
                MovimentEnemic();
            }
            else
            {
                movimentEnemicAEstrella();
            }
        }
    }

    private void MovimentEnemic()
    {
        // Verifica si hay obstáculos antes de mover al enemigo
        Vector2 direccionMovimiento = direcciones[indiceDireccion];
        Vector2 proximaPosicion = (Vector2)transform.position + direccionMovimiento * velocidad * Time.deltaTime;

        // Detecta si el enemigo va a chocar con algo usando un raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionMovimiento, velocidad * Time.deltaTime);

        if (hit.collider != null && hit.collider.CompareTag("Pared"))  // Si el raycast toca una pared
        {
            // Si hay un obstáculo, cambia la dirección para evitar quedarse atascado
            CambiarDireccionAleatoria();
        }
        else
        {
            // Mueve al enemigo de acuerdo a la dirección elegida, sin interferencias
            rb.velocity = direccionMovimiento * velocidad;
        }
    }

    private void CambiarDireccionAleatoria()
    {
        // Elige una nueva dirección aleatoria para el enemigo
        indiceDireccion = Random.Range(0, direcciones.Length);
    }

    // Nueva implementación de A* (estrella) para seguir al jugador, manteniendo el código original intacto
    private void movimentEnemicAEstrella()
    {
        if (personatge == null)
        {
            Debug.LogWarning("La variable 'personatge' no está asignada. Asigna el jugador en el Inspector.");
            return;
        }

        // Si no hay camino o el jugador se ha movido, recalcula el camino
        if (path == null || path.Count == 0 || HaCambiadoPosicionJugador())
        {
            CalcularCaminoAEstrella();
        }

        // Sigue el camino calculado
        SeguirCamino();
    }

    // Variables adicionales para A*
    private List<Vector2> path = new List<Vector2>(); // Camino calculado
    private int currentPathIndex = 0; // Índice del punto actual en el camino
    public LayerMask obstacleLayer; // Capa para detectar obstáculos (configurar en el Inspector)

    // Verifica si el jugador ha cambiado de posición significativamente
    private bool HaCambiadoPosicionJugador()
    {
        if (path != null && path.Count > 0)
        {
            return Vector2.Distance(path[path.Count - 1], personatge.transform.position) > 0.5f;
        }
        return true;
    }

    // Implementación simplificada de A* para un espacio 2D continuo
    private void CalcularCaminoAEstrella()
    {
        path.Clear();
        Vector2 start = transform.position;
        Vector2 goal = personatge.transform.position;

        // Lista de puntos abiertos (nodos por explorar)
        List<Vector2> openSet = new List<Vector2> { start };
        // Lista de puntos cerrados (nodos ya explorados)
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        // Diccionario para rastrear el padre de cada punto
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        // Costes g (desde el inicio) y h (heurística hasta el objetivo)
        Dictionary<Vector2, float> gScores = new Dictionary<Vector2, float> { { start, 0 } };
        Dictionary<Vector2, float> fScores = new Dictionary<Vector2, float> { { start, Heuristica(start, goal) } };

        while (openSet.Count > 0)
        {
            // Encuentra el punto con el menor coste f
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

            // Explora los vecinos (puntos cercanos en 8 direcciones)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 neighbor = current + new Vector2(i * 0.5f, j * 0.5f); // Movimiento en pasos pequeños

                    // Verifica si hay un obstáculo entre el punto actual y el vecino
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

        path = null; // No se encontró un camino
    }

    // Heurística (distancia euclidiana)
    private float Heuristica(Vector2 from, Vector2 to)
    {
        return Vector2.Distance(from, to);
    }

    // Verifica si hay un obstáculo entre dos puntos usando raycast
    private bool HayObstaculo(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleLayer);
        return hit.collider != null;
    }

    // Reconstruye el camino desde el inicio hasta el objetivo
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

    // Mueve al enemigo siguiendo el camino calculado
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

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void DieAnim()
    {
        questContainer.GetComponent<ShowQuest>().IncrementCounter(1);
        Destroy(gameObject);  // Elimina al enemigo
    }

    // Enumeración para alternar entre métodos de actualización
    private enum UpdateMethod
    {
        Original,
        AStar
    }

    private UpdateMethod updateMethod = UpdateMethod.Original; // Por defecto, usa el movimiento original
}
  