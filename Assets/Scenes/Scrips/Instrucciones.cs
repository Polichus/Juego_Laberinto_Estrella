using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instrucciones : MonoBehaviour
{
    public void AnaraAEscenaInstruccions()
    {
        SceneManager.LoadScene("Instrucciones");
    }

    public void AnarAEscenaPrincipal()
    {
        SceneManager.LoadScene("Pantalla Principal");
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
