using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int monedasTotales;
    public int nivelMaximoAlcanzado; // Nueva variable

    void Awake()
{
    if (instance == null)
    {
        instance = this;
        // Esta línea es MAGIA: hace que el GameManager no muera 
        // cuando cambies de la EscenaPrincipal al Nivel2
        DontDestroyOnLoad(gameObject); 
        CargarProgreso();
    }
    else
    {
        // Si ya existe uno, destruimos el nuevo para que no haya dos contadores
        Destroy(gameObject);
    }
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
        // Solo actualizamos si el nivel que pasamos es mayor al que ya teníamos guardado
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
        PlayerPrefs.SetInt("NivelMaximo", nivelMaximoAlcanzado); // Guardamos el nivel
        PlayerPrefs.Save(); 
    }

    void CargarProgreso()
    {
        monedasTotales = PlayerPrefs.GetInt("MonedasGuardadas", 0);
        nivelMaximoAlcanzado = PlayerPrefs.GetInt("NivelMaximo", 1); // Por defecto empieza en el 1
    }
}