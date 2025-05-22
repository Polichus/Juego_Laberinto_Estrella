using UnityEngine;

public class EnemigoPulpo : MonoBehaviour
{
    public Transform[] waypoints;
    public float velocidad = 2f;
    private int waypointIndex = 0;
    private ShowQuest showQuest;
    public int salud = 1;

    void Start()
    {
        showQuest = FindObjectOfType<ShowQuest>();  // ✅ Referencia al sistema de misiones

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[waypointIndex].position;
        }
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
                Debug.Log("Pulpo eliminado.");

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