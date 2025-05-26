using TMPro;
using UnityEngine;

public class ShowQuest : MonoBehaviour
{
    [Header("Asignar en Inspector")]
    public GameObject questPanel;
    public TextMeshProUGUI contador_1;
    public TextMeshProUGUI contador_2;
    public TextMeshProUGUI contador_3;
    public GameObject tick1;
    public GameObject tick2;
    public GameObject tick3;


    public TextMeshProUGUI textoRestante; // <- AÑADIDO: texto "Te falta matar..."

    //public int enemiesKilled = 7;
    public int totalEnemiesKilled;

    void Start()
    {
        questPanel?.SetActive(false);
        ResetCounters();
        ActualizarTextoRestante(); // <- Actualiza texto al inicio
        ValorsGlobals.enemigos_restantes = 7;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleQuestPanel();
        }

    }

    public void AddKill()
    {
        totalEnemiesKilled++;
        ValorsGlobals.enemigos_restantes--;

        if (ValorsGlobals.enemigos_restantes < 0)
            ValorsGlobals.enemigos_restantes = 0;

        Debug.Log($"Enemigos restantes: {ValorsGlobals.enemigos_restantes}");

        UpdateCounters();
        CheckQuestCompletion();
        ActualizarTextoRestante(); // <- Actualiza el texto aquí
    }

    void UpdateCounters()
    {
        int progress1 = Mathf.Clamp(totalEnemiesKilled, 0, 3);
        contador_1.text = $"{progress1} / 3";

        int progress2 = Mathf.Clamp(totalEnemiesKilled - 3, 0, 2);
        contador_2.text = $"{progress2} / 2";

        int progress3 = Mathf.Clamp(totalEnemiesKilled - 5, 0, 2);
        contador_3.text = $"{progress3} / 2";
    }

    void ToggleQuestPanel()
    {
        if (questPanel != null)
            questPanel.SetActive(!questPanel.activeSelf);
    }

    void CheckQuestCompletion()
    {
        if (tick1 != null && totalEnemiesKilled >= 3)
        {
            tick1.SetActive(true);
            Debug.Log("Misión 1 completada!");
        }
        if (tick2 != null && totalEnemiesKilled >= 5)
        {
            tick2.SetActive(true);
            Debug.Log("Misión 2 completada!");
        }
        if (tick3 != null && totalEnemiesKilled >= 7)
        {
            tick3.SetActive(true);
            Debug.Log("Misión 3 completada!");
        }
    }




    void ResetCounters()
    {
        totalEnemiesKilled = 0;
        ValorsGlobals.enemigos_restantes = 7;
        UpdateCounters();
    }

    void ActualizarTextoRestante()
    {
        if (textoRestante != null)
        {
            textoRestante.text = $"Te falta matar: {ValorsGlobals.enemigos_restantes} enemigo(s)";
            Debug.Log($"Texto actualizado directamente desde ShowQuest: {textoRestante.text}");
        }
    }

    public int getScore()
    {
        return totalEnemiesKilled;
    }
}
