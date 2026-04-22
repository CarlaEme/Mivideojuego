using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas si decides volver al menú


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int monedasTotales;
    public int nivelMaximoAlcanzado;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            CargarProgreso();
        }
        else
        {
            Destroy(gameObject);

            
        }
    }

    // --- SECCIÓN DE CONTROL ---
    void Update()
    {
        // Si presionas la tecla "Escape", el juego se cierra
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SalirDelJuego();
        }
    }

    // Esta función sirve tanto para la tecla ESC como para un botón de "Salir"
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit(); 
    }

    // --- SECCIÓN DE MONEDAS ---
    public void SumarMoneda()
    {
        monedasTotales++;
        GuardarProgreso();
    }

    // --- SECCIÓN DE NIVELES ---
    public void DesbloquearSiguienteNivel(int numeroDeNivel)
    {
        if (numeroDeNivel > nivelMaximoAlcanzado)
        {
            nivelMaximoAlcanzado = numeroDeNivel;
            GuardarProgreso();
            Debug.Log("Nuevo nivel desbloqueado: " + nivelMaximoAlcanzado);
        }
    }

    // --- GUARDADO Y CARGA ---
    public void GuardarProgreso()
    {
        PlayerPrefs.SetInt("MonedasGuardadas", monedasTotales);
        PlayerPrefs.SetInt("NivelMaximo", nivelMaximoAlcanzado);
        PlayerPrefs.Save(); 
    }

    void CargarProgreso()
    {
        monedasTotales = PlayerPrefs.GetInt("MonedasGuardadas", 0);
        nivelMaximoAlcanzado = PlayerPrefs.GetInt("NivelMaximo", 1);
    }
}