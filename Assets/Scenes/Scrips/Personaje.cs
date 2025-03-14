using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float speed = 3f;
    public Animator animator;  // Cambiado de Animation a Animator
    public SpriteRenderer spriteRenderer;
    public GameObject prefab_proyectil;
    private float projectileSpeed = 10f;
    private Vector2 ultimaDireccio = Vector2.down;

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(h, v, 0);

        // Control de animaciones
        if (h > 0)
        {
            animator.Play("caminando_derecha");  // Nombre exacto de la animación
        }
        else if (h < 0)
        {
            animator.Play("caminando_izquierda");
        }
        else if (v > 0)
        {
            animator.Play("caminando_arriba");
        }
        else if (v < 0)
        {
            animator.Play("caminando_abajo");
        }
        else
        {
            animator.Play("idle"); // Animación en reposo
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Disparo con clic o botón
        {
            Shoot(h, v);
        }

    }



    public void Shoot(float h, float v)
    {
        // Crear el proyectil
        GameObject projectile = Instantiate(prefab_proyectil);
        projectile.transform.position = transform.position;
        //projectile.transform.rotation = Vector2.right;

        // Determinar la dirección en la que el jugador está mirando
        Vector2 direction = Vector2.zero;

        if (h == 0 && v == 0)
        {
            direction = ultimaDireccio;
        }

        if (h > 0)
        {
            direction = Vector2.right;
            ultimaDireccio = direction;
        }
        else if (h < 0)
        {
            direction = Vector2.left;
            ultimaDireccio = direction;
        }
        else if (v > 0)
        {
            direction = Vector2.up;
            ultimaDireccio = direction;
        }
        else if (v < 0)
        {
            direction = Vector2.down;
            ultimaDireccio = direction;
        }

        
          
        if (h > 0 && v < 0)
        {
            direction = Vector2.Lerp(Vector2.right, Vector2.down, 0.5f);
            ultimaDireccio = direction;
        }
        else if (h > 0 && v > 0)
        {
            direction = Vector2.Lerp(Vector2.right, Vector2.up ,0.5f);
            ultimaDireccio = direction;
        }
        else if (h < 0 && v < 0)
        {
            direction = Vector2.Lerp(Vector2.left, Vector2.down, 0.5f);
            ultimaDireccio = direction;
        }
        else if (h < 0 && v > 0)
        {
            direction = Vector2.Lerp(Vector2.left, Vector2.up, 0.5f);
            ultimaDireccio = direction;
        }

        
        // Convertir la dirección (Vector2) en un ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Asignar la rotación al proyectil (Quaternion)
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 90);  

        // Asignar la dirección y velocidad al proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log("resultat: " + direction);
            rb.velocity = direction * projectileSpeed;
        }
    }
}

