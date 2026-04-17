using UnityEngine;
using UnityEngine.SceneManagement; 

public class FinalNivel : MonoBehaviour
{
    public int nivelParaDesbloquear = 2;
    public string nombreDelSiguienteNivel = "Nivel2"; 
    public string nombreNivelEnFirebase = "nivel1"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Guardado Local
            if (GameManager.instance != null)
            {
                GameManager.instance.DesbloquearSiguienteNivel(nivelParaDesbloquear);
                GameManager.instance.GuardarProgreso();
            }

            // 2. Guardado en la Nube (Firebase)
            FirebaseManager fbManager = FindObjectOfType<FirebaseManager>();
            if (fbManager != null)
            {
                fbManager.GuardarNivelCompletado(nombreNivelEnFirebase);
                Debug.Log("Enviando " + nombreNivelEnFirebase + " a la nube...");

                
            }

            // 3. Esperar medio segundo y cambiar de escena
            // Esto evita que la escena se cierre antes de que llegue el dato a Google
           
            Invoke("CambiarEscena", 0.5f); 
        }
    }

    // Esta función debe estar DENTRO de las llaves de la clase
    void CambiarEscena()
    {
        SceneManager.LoadScene(nombreDelSiguienteNivel);
    }
}