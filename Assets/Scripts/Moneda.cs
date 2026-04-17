using UnityEngine;

public class Moneda : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Importante: Tu personaje debe tener el Tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Buscamos al FirebaseManager para que suba el dato a la nube
            FirebaseManager fbManager = FindObjectOfType<FirebaseManager>();
            
            if (fbManager != null)
            {
                fbManager.SumarMoneda();
                Debug.Log("Moneda recogida y enviada a Firebase");
            }

            // Destruimos la moneda física del juego
            Destroy(gameObject);
        }
    }
}