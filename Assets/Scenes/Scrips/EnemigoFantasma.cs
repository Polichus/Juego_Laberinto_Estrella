using UnityEngine;

public class EnemigoFantasma : MonoBehaviour
{
    public Transform[] waypoints;
    public float velocidad = 2f;
    private int waypointIndex = 0;
    public int salud;
    private ShowQuest showQuest;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[waypointIndex].position;
        }

        // ✅ Obtener referencia al script de misiones
        showQuest = FindObjectOfType<ShowQuest>();
    }

    void Update()
    {
        if (waypoints.Length > 0)
        {
            MoverPorWaypoints();
        }
    }

    void MoverPorWaypoints()
    {
        Transform objetivo = waypoints[waypointIndex];
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;

        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Shooting"))
        {
            if (salud > 0)
            {
                salud--;
            }
            else
            {
                Debug.Log("Fantasma1 eliminado.");

                // ✅ Sumar kill al sistema de misiones
                if (showQuest != null)
                {
                    showQuest.AddKill();
                }

                Destroy(gameObject);  // Destruir enemigo
            }
        }
    }
}