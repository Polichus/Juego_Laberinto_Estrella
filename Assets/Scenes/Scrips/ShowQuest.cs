using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowQuest : MonoBehaviour
{
    public GameObject quest;
    public TextMeshProUGUI contador_1;
    public TextMeshProUGUI contador_2;
    public TextMeshProUGUI contador_3;
    public SpriteRenderer[] tickSprite;
    public Sprite tick;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        quest.SetActive(false);
        
        contador_1.text = "0 / 3";
      
        contador_2.text = "0 / 5";
       
        contador_3.text = "0 / 7";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))        
            quest.SetActive(true);
        if(Input.GetKeyUp(KeyCode.Tab))
            quest.SetActive(false);
        UpdateText();
    }

    public void IncrementCounter(int value) 
    {
       counter += value;

    }
    private void UpdateText()
    {
        if(counter <= 3)
            contador_1.text = counter.ToString() + " / 3";
        if (counter >3 && counter <= 5)
            contador_2.text = counter.ToString() + " / 5";
      
        if(counter > 5 && counter <= 7)
            contador_3.text = counter.ToString() + " / 7";
        
    }
    public void addTick(int valor)
    {
        if(valor == 1)
        {
            tickSprite[0].sprite = tick;
        }
        if (valor == 2)
        {
            tickSprite[1].sprite = tick;
        }
        if (valor == 3)
        {
            tickSprite[2].sprite = tick;
        }
    }

    public int  getScore()
    {
        return counter;
    }


}
