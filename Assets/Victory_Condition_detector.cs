using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory_Condition_detector : MonoBehaviour
{
    public GameObject[] panels;      // [0] = victoria, [1] = misiones incompletas
    public TextMeshProUGUI tequeda;  // Texto panel misiones incompletas

    //public int enemiesLeft = 7;      // <- CONTROL DIRECTO DE LOS ENEMIGOS
    private bool gameEnded = false;

    void Start()
    {
        
        foreach (GameObject panel in panels)
            panel.SetActive(false);

        UpdateEnemyText();
    }

    void UpdateEnemyText()
    {
        if (tequeda != null)
        {
            tequeda.text = $"Te falta matar: {ValorsGlobals.enemigos_restantes} enemigo(s)";
            Debug.Log($"Texto actualizado: {tequeda.text}");
        }
    }

    public void EnemyKilled()
    {
        ValorsGlobals.enemigos_restantes--;
        if (ValorsGlobals.enemigos_restantes < 0) ValorsGlobals.enemigos_restantes = 0;
        UpdateEnemyText();
    }


    private void EndGame()
    {
        gameEnded = true;
        panels[1].SetActive(false);
        panels[0].SetActive(true);
        StartCoroutine(VolverAlMenu());
    }

    IEnumerator VolverAlMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Pantalla Principal");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ValorsGlobals.enemigos_restantes <= 0)
            {
                EndGame();
            }
            else
            {
                panels[0].SetActive(false);
                panels[1].SetActive(true);
                Debug.Log($"Enemigos restantes al entrar al trigger: {ValorsGlobals.enemigos_restantes}");
                UpdateEnemyText();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (panels != null && panels.Length > 1 && panels[1] != null)
            {
                panels[1].SetActive(false);
            }
        }
    }
}
