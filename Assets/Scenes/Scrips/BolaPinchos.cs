using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaPinchos : MonoBehaviour
{
    public Transform[] waypoints;          // Lista de puntos por los que se moverá el enemigo
    public float velocidad = 2f;           // Velocidad de movimiento
    private int waypointIndex = 0;         // Índice del waypoint actual
    public float velocidad_giro = 10f;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No se han asignado waypoints al enemigo.");
            enabled = false;
            return;
        }

        transform.position = waypoints[waypointIndex].position; // Comenzar en el primer waypoint
    }

    void Update()
    {
        MoverPorWaypoints();
    }

    void MoverPorWaypoints()
    {
        if (waypoints.Length == 0) return;

        Transform objetivo = waypoints[waypointIndex];
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;
        transform.Rotate(0f, 0f, velocidad_giro * Time.deltaTime);

        // Cuando llega cerca del waypoint, pasa al siguiente
        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length; // Reinicia cuando llega al final
        }
    }
}
