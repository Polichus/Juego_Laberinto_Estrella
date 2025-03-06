using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NIveles : MonoBehaviour
{

    public void AnarAEscenaNivelFacil()
    {
        SceneManager.LoadScene("Mapa nivel facil");
    }

    public void AnarAEscenaNivelExperto()
    {
        SceneManager.LoadScene("Mapa nivel experto");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
