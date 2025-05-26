using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float speed = 3f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject prefab_proyectil;
    private float projectileSpeed = 15f;
    private Vector2 ultimaDireccio = Vector2.down;
    public GameObject posInicial; // Punto de reaparición

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegura que el personaje empiece en la posición inicial
        if (posInicial != null)
        {
            transform.position = posInicial.transform.position;
        }
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(h, v, 0);

        // Animaciones
        if (h > 0) animator.Play("caminando_derecha");
        else if (h < 0) animator.Play("caminando_izquierda");
        else if (v > 0) animator.Play("caminando_arriba");
        else if (v < 0) animator.Play("caminando_abajo");
        else animator.Play("idle");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(h, v);
        }
    }

    public void Shoot(float h, float v)
    {
        GameObject projectile = Instantiate(prefab_proyectil);
        projectile.transform.position = transform.position;

        Vector2 direction = Vector2.zero;

        if (h == 0 && v == 0) direction = ultimaDireccio;

        if (h > 0) direction = Vector2.right;
        else if (h < 0) direction = Vector2.left;
        else if (v > 0) direction = Vector2.up;
        else if (v < 0) direction = Vector2.down;

        if (h > 0 && v < 0) direction = Vector2.Lerp(Vector2.right, Vector2.down, 0.5f);
        else if (h > 0 && v > 0) direction = Vector2.Lerp(Vector2.right, Vector2.up, 0.5f);
        else if (h < 0 && v < 0) direction = Vector2.Lerp(Vector2.left, Vector2.down, 0.5f);
        else if (h < 0 && v > 0) direction = Vector2.Lerp(Vector2.left, Vector2.up, 0.5f);

        ultimaDireccio = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            // Teletransportar al personaje a la posición inicial
            if (posInicial != null)
            {
                transform.position = posInicial.transform.position;
                Debug.Log("¡Has tocado una trampa! Regresando a inicio.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado posInicial.");
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("Trampa"))
        {
            // Teletransportar al personaje a la posición inicial
            if (posInicial != null)
            {
                transform.position = posInicial.transform.position;
                Debug.Log("¡Has tocado una trampa! Regresando a inicio.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado posInicial.");
            }
        }

        if (collision.gameObject.CompareTag("Bola"))
        {
            // Teletransportar al personaje a la posición inicial
            if (posInicial != null)
            {
                transform.position = posInicial.transform.position;
                Debug.Log("¡Has tocado una Bola! Regresando a inicio.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado posInicial.");
            }
        }

        if (collision.gameObject.CompareTag("Pulpo"))
        {
            // Teletransportar al personaje a la posición inicial
            if (posInicial != null)
            {
                transform.position = posInicial.transform.position;
                Debug.Log("¡Has tocado al Pulpo! Regresando a inicio.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado posInicial.");
            }
        }

        if (collision.gameObject.CompareTag("Fantasma1"))
        {
            // Teletransportar al personaje a la posición inicial
            if (posInicial != null)
            {
                transform.position = posInicial.transform.position;
                Debug.Log("¡Has tocado un Fantasma! Regresando a inicio.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado posInicial.");
            }
        }
    }
}
