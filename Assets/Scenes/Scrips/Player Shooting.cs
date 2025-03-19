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

    public class Projectile : MonoBehaviour
    {
        internal float projectileSpeed;
        internal float lifetime;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
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
    }
}
