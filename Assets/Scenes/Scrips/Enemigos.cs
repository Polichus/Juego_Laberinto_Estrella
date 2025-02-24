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

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa el enemigo en una dirección aleatoria
        CambiarDireccionAleatoria();
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve al enemigo en la dirección seleccionada
        transform.Translate(direcciones[indiceDireccion] * velocidad * Time.deltaTime);

        // Cuenta el tiempo para cambiar de dirección
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoCambio)
        {
            tiempoTranscurrido = 0f;  // Reinicia el tiempo
            CambiarDireccionAleatoria();  // Cambia la dirección aleatoriamente
        }
    }

    // Función para cambiar la dirección aleatoriamente
    private void CambiarDireccionAleatoria()
    {
        // Elige un índice aleatorio entre las direcciones posibles
        indiceDireccion = Random.Range(0, direcciones.Length);  // Random.Range(0, 4) da un número entre 0 y 3
    }
}
