using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Victory_Condition_detector : MonoBehaviour
{
    GameObject showQuest;
    public GameObject [] panels;
    public GameObject tequeda;
    void Start()
    {
        showQuest = GameObject.Find("QuestContainer");
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void EndGame()
    {
        panels[0].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (showQuest != null)
                if (showQuest.GetComponent<ShowQuest>().getScore() == 7)
                    EndGame();

            if (showQuest.GetComponent<ShowQuest>().getScore() < 7)
            {
                int maxEnemyToKill = 7;
                maxEnemyToKill -= GetComponent<ShowQuest>().getScore();
                tequeda.GetComponent<TextMeshPro>().text = maxEnemyToKill.ToString();
                panels[1].SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (showQuest.GetComponent<ShowQuest>().getScore() < 7)
            {
                panels[1].SetActive(false);
            }
        }
    }
}
