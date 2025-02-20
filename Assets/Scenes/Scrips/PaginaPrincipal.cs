using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaginaPrincipal : MonoBehaviour
{
    public void AnarAEscenaJugant()
    {
        SceneManager.LoadScene("Niveles");
    }
    public void AnaraAEscenaInstruccions()
    {
        SceneManager.LoadScene("Instrucciones");
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
