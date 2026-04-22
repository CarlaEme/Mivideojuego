using UnityEngine;
using TMPro; // Si usas TextMeshPro
using UnityEngine.UI; // Si usas el Text normal de Unity
using UnityEngine.SceneManagement;

public class MostradorNivel : MonoBehaviour
{
    public TMP_Text textoNivelTMP; // Arrastra aquí tu objeto si es TextMeshPro
    public Text textoNivelNormal;  // Arrastra aquí tu objeto si es Text normal

    void Start()
    {
        // Obtenemos el nombre de la escena actual
        string nombreEscena = SceneManager.GetActiveScene().name;
        
        // Actualizamos el texto
        if (textoNivelTMP != null) textoNivelTMP.text = "Nivel " + nombreEscena.Replace("Nivel", "");
        if (textoNivelNormal != null) textoNivelNormal.text = nombreEscena;
    }
}

