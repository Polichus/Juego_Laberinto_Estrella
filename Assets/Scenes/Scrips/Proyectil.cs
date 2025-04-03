using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform shootPoint; // Punto desde donde se dispara
    public float projectileSpeed = 10f;
    public Enemigos enemies;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Enemigos enemigo = collision.GetComponent<Enemigos>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(1);
            }
            //collision.GetComponent<Enemigos>().TakeDamage(1);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }
    }

}
