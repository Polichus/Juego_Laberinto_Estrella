using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public float velocidad = 3f;  // Velocidad de movimiento del enemigo
    private Vector3[] direcciones = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };  // Direcciones posibles
    private int indiceDireccion = 0;  // �ndice de la direcci�n actual
    private float tiempoCambio = 2f;  // Tiempo antes de cambiar de direcci�n
    private float tiempoTranscurrido = 0f;
    public GameObject personatge;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa el enemigo en una direcci�n aleatoria
        CambiarDireccionAleatoria();
    }

    // Update is called once per frame
    void Update()
    {
        // Opcionalmente, puedes mantener uno por defecto y alternar con la tecla "T":
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Alterna entre los dos m�todos
            if (updateMethod == UpdateMethod.Original)
            {
                updateMethod = UpdateMethod.AStar;
            }
            else
            {
                updateMethod = UpdateMethod.Original;
            }
        }

        // Ejecuta el m�todo seleccionado
        if (updateMethod == UpdateMethod.Original)
        {
            movimentEnemic();
        }
        else
        {
            movimentEnemicAEstrella();
        }
    }

    private void MovimentAleatori()
    {
        // Mueve al enemigo en la direcci�n seleccionada
        transform.Translate(direcciones[indiceDireccion] * velocidad * Time.deltaTime);

        // Cuenta el tiempo para cambiar de direcci�n
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoCambio)
        {
            tiempoTranscurrido = 0f;  // Reinicia el tiempo
            CambiarDireccionAleatoria();  // Cambia la direcci�n aleatoriamente
        }
    }

    // Funci�n para cambiar la direcci�n aleatoriamente
    private void CambiarDireccionAleatoria()
    {
        // Elige un �ndice aleatorio entre las direcciones posibles
        indiceDireccion = Random.Range(0, direcciones.Length);  // Random.Range(0, 4) da un n�mero entre 0 y 3
    }

    private void movimentEnemic()
    {
        Vector2 direccioEnemic = (personatge.transform.position - transform.position).normalized;
        Vector2 novaPosEnemic = new Vector2(transform.position.x + direccioEnemic.x * velocidad * Time.deltaTime, transform.position.y + direccioEnemic.y * velocidad * Time.deltaTime);
        Debug.Log(novaPosEnemic);
        transform.position = novaPosEnemic;
    }

    // Nueva implementaci�n de A* (estrella) para seguir al jugador, manteniendo el c�digo original intacto
    private void movimentEnemicAEstrella()
    {
        if (personatge == null)
        {
            Debug.LogWarning("La variable 'personatge' no est� asignada. Asigna el jugador en el Inspector.");
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
    private int currentPathIndex = 0; // �ndice del punto actual en el camino
    public LayerMask obstacleLayer; // Capa para detectar obst�culos (configurar en el Inspector)

    // Verifica si el jugador ha cambiado de posici�n significativamente
    private bool HaCambiadoPosicionJugador()
    {
        if (path != null && path.Count > 0)
        {
            return Vector2.Distance(path[path.Count - 1], personatge.transform.position) > 0.5f;
        }
        return true;
    }

    // Implementaci�n simplificada de A* para un espacio 2D continuo
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
        // Costes g (desde el inicio) y h (heur�stica hasta el objetivo)
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

                    Vector2 neighbor = current + new Vector2(i * 0.5f, j * 0.5f); // Movimiento en pasos peque�os

                    // Verifica si hay un obst�culo entre el punto actual y el vecino
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

        path = null; // No se encontr� un camino
    }

    // Heur�stica (distancia euclidiana)
    private float Heuristica(Vector2 from, Vector2 to)
    {
        return Vector2.Distance(from, to);
    }

    // Verifica si hay un obst�culo entre dos puntos usando raycast
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

    // Enumeraci�n para alternar entre m�todos de actualizaci�n
    private enum UpdateMethod
    {
        Original,
        AStar
    }

    private UpdateMethod updateMethod = UpdateMethod.Original; // Por defecto, usa el movimiento original
}
