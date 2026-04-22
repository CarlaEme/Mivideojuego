using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Función para el botón Jugar
    public void Jugar()
    {
        // "EscenaPrincipal" debe ser el nombre exacto de tu nivel 1
        SceneManager.LoadScene("Nivel1");
    }

    // Función para el botón Salir
    public void Salir()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit(); 
    }
}

