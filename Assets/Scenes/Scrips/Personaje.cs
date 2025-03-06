using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float speed = 3f;
    public Animator animator;  // Cambiado de Animation a Animator
    public SpriteRenderer spriteRenderer;

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
    }
}

