using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform shootPoint; // Punto desde donde se dispara
    public float projectileSpeed = 10f;

    void Update()
    {

    }

    /*public class Projectile : MonoBehaviour
    {
        internal float projectileSpeed;
        internal float lifetime;
        GameObject[] enemies;
        
        private void GetEnemies()
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemigo in enemies)
            {
                Debug.Log("Encontrado: " + enemigo.name);
            }

            //Enemy enemy = collision.GetComponent<Enemy>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.gameObject.tag=="enemy")
            {
                enemy.TakeDamage();
                Destroy(gameObject); // Destruir proyectil al impactar
            }
        }
    }

    public class Enemy : MonoBehaviour
    {
        public int health = 1;
        public Animator animator;

        public void TakeDamage()
        {
            health--;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        IEnumerator Die()
        {
            animator.SetTrigger("Die"); // Ejecuta animación de muerte
            yield return new WaitForSeconds(1f); // Espera que termine la animación
            Destroy(gameObject);
        }
    }*/
}
