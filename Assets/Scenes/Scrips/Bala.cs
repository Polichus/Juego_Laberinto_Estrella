using UnityEngine;

public class Bala : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            FindObjectOfType<ShowQuest>()?.AddKill();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }
    }
}