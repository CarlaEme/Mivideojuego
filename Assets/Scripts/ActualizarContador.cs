using UnityEngine;
using TMPro; // Es muy importante que diga esto arriba

public class ActualizarContador : MonoBehaviour
{
    private TMP_Text componenteTexto;

    void Start()
    {
        // Buscamos el componente de texto en este mismo objeto
        componenteTexto = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (GameManager.instance != null)
        {
            // Actualizamos el letrero con el valor real del GameManager
            componenteTexto.text = "Monedas: " + GameManager.instance.monedasTotales;
        }
    }
}