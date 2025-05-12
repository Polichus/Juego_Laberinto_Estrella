using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoRuta : MonoBehaviour
{
    public Transform[] waypoints;          // Lista de puntos por los que se moverá el enemigo
    public float velocidad = 2f;           // Velocidad de movimiento
    private int waypointIndex = 0;         // Índice del waypoint actual

    public int salud = 1;                  // Salud del enemigo
    private ShowQuest showQuest;           // Referencia al script ShowQuest

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No se han asignado waypoints al enemigo.");
            enabled = false;
            return;
        }

        transform.position = waypoints[waypointIndex].position; // Comenzar en el primer waypoint

        // Obtener la referencia al script ShowQuest
        showQuest = FindObjectOfType<ShowQuest>();
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

        // Cuando llega cerca del waypoint, pasa al siguiente
        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length; // Reinicia cuando llega al final
        }
    }

    // Método para recibir daño
    public void RecibirDanio(int cantidad)
    {
        salud -= cantidad;

        if (salud <= 0)
        {
            Morir();
        }
    }

    // Método que se llama cuando el enemigo muere
    private void Morir()
    {
        // Llamar al método para incrementar el contador de misiones (suponiendo que 1 es el valor de incremento)
        if (showQuest != null)
        {
          //  showQuest.IncrementCounter(1);
        }

        // Aquí puedes agregar otras lógicas, como efectos de muerte, animaciones, etc.
        Destroy(gameObject);  // Elimina el objeto del juego
    }
}
