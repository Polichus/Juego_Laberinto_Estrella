using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Daño a EnemigoFantasma
        /*EnemigoFantasma enemigo = collision.GetComponent<EnemigoFantasma>();
        if (enemigo != null)
        {
            enemigo.RecibirDaño(1);
            Destroy(gameObject);
            return;
        }*/

        // Destruye si choca con una pared
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
