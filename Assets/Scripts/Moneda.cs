using UnityEngine;

public class Moneda : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Actualizamos en Firebase (Nube)
            FirebaseManager fbManager = FindObjectOfType<FirebaseManager>();
            if (fbManager != null)
            {
                fbManager.SumarMoneda();
                Debug.Log("Moneda enviada a Firebase");
            }

            // 2. Actualizamos el GameManager (Local para la interfaz)
            // ESTA ES LA LÍNEA QUE TE FALTA PARA QUE EL CONTADOR SE MUEVA
            if (GameManager.instance != null)
            {
                GameManager.instance.SumarMoneda();
            }

            Destroy(gameObject);
        }
    }
}