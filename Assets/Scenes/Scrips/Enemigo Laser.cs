using System.Collections;
using UnityEngine;

public class EnemigoLaser : MonoBehaviour
{
    public Animator laserAnim;
    private Collider2D laserCol;
    public float initTimer = 0.8f;
    private float laserTimer;
    private float activationCol;
    private bool laserActive = true;
    private float laserRecovery = 3.5f;
    private Personaje personaje;
    private SpriteRenderer myr;

    void Start()
    {
        laserTimer = initTimer;
        laserCol = GetComponent<Collider2D>();
        personaje = FindAnyObjectByType<Personaje>();
        myr = GetComponent<SpriteRenderer>();

        // Establecer la velocidad de la animación (más lento)
        laserAnim.speed = 0.5f;

        // Activar animación y colisión
        laserAnim.Play("Laser_Anim");
        laserCol.enabled = true;

        activationCol = initTimer / 2;
    }

    void Update()
    {

        if (laserActive) {
            laserAnim.speed = 0.8f; // reanudar animación
            //laserAnim.Play("Laser_Anim", 0, 0f); // reiniciar desde el inicio
            laserTimer -= Time.deltaTime;
            activationCol -= Time.deltaTime;
            if(activationCol < 0)
            {
                laserCol.enabled = true;
            }
            if (laserTimer < 0) { 
                laserActive = false;
                myr.enabled = false;

            }
        }
        else
        {
            laserAnim.speed = 0f;
            laserCol.enabled = false;
            laserRecovery -=Time.deltaTime;

            if (laserRecovery < 0)
            {
                laserTimer = initTimer;
                activationCol = initTimer / 2;
                laserRecovery = 0.5f; // Reiniciamos el cooldown
                laserActive = true;
                myr.enabled = true;
                laserAnim.Play("Laser_Anim", 0, 0f);

            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            personaje.transform.position = personaje.posInicial.gameObject.transform.position;
        }
    }
}
