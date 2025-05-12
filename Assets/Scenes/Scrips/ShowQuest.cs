using TMPro;
using UnityEngine;

public class ShowQuest : MonoBehaviour
{
    [Header("Asignar en Inspector")]
    public GameObject questPanel; // Panel padre de las misiones
    public TextMeshProUGUI contador_1; // "0/3 enemigos"
    public TextMeshProUGUI contador_2; // "0/5 enemigos"
    public TextMeshProUGUI contador_3; // "0/7 enemigos"

    private int totalEnemiesKilled; // Contador TOTAL de enemigos eliminados

    void Start()
    {
        questPanel?.SetActive(false); // Oculta el panel al inicio (el ? evita nulls)
        ResetCounters();
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
        UpdateCounters();

        // Opcional: Notificar cuando se completa una misión
        CheckQuestCompletion();
    }

    void UpdateCounters()
    {
        // Misión 1: 0-3 enemigos
        int progress1 = Mathf.Clamp(totalEnemiesKilled, 0, 3);
        contador_1.text = $"{progress1} / 3";

        // Misión 2: 3-8 enemigos (5 adicionales)
        int progress2 = Mathf.Clamp(totalEnemiesKilled - 3, 0, 5);
        contador_2.text = $"{progress2} / 5";

        // Misión 3: 8-15 enemigos (7 adicionales)
        int progress3 = Mathf.Clamp(totalEnemiesKilled - 8, 0, 7);
        contador_3.text = $"{progress3} / 7";
    }

    void ToggleQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(!questPanel.activeSelf);
        }
    }

    void CheckQuestCompletion()
    {
        if (totalEnemiesKilled == 3) Debug.Log("Misión 1 completada!");
        if (totalEnemiesKilled == 5) Debug.Log("Misión 2 completada!");
        if (totalEnemiesKilled == 7) Debug.Log("Misión 3 completada!");
    }

    void ResetCounters()
    {
        totalEnemiesKilled = 0;
        UpdateCounters();
    }

    public int getScore()
    {
        return totalEnemiesKilled;
    }
}