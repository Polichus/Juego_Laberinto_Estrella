using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class cierreeditor : MonoBehaviour
{

    public void QuitEditor()
    {
        EditorApplication.isPlaying = false;
        EditorApplication.Exit(0);
    }

    public void AnaraAEscenaPrincipal()
    {
        SceneManager.LoadScene("Pantalla Principal");
    }

    public void AnaraAEscenaSalir()
    {
        SceneManager.LoadScene("pantalla salir");
    }

}
