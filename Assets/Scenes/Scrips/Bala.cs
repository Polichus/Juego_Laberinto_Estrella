using UnityEngine;

public class Bala : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con un enemigo común
        if (collision.CompareTag("enemy"))
        {
            FindObjectOfType<ShowQuest>()?.AddKill();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        // Si colisiona con un enemigo "Fantasma1"
        if (collision.CompareTag("Fantasma1"))
        {
            Destroy(gameObject);
        }

        // Si colisiona con un enemigo "Fantasma2"
        if (collision.CompareTag("Fantasma2"))
        {
            Destroy(gameObject);
        }

        // Si colisiona con un enemigo "Pulpo"
        if (collision.CompareTag("Pulpo"))
        {
            Destroy(gameObject);
        }

        // Si colisiona con una pared
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
