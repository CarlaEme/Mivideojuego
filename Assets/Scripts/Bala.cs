using UnityEngine;

public class Bala : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la bala toca cualquier cosa que no sea el jugador...
        if (!collision.CompareTag("Player"))
        {
            // ¡Pum! Se destruye
            Destroy(gameObject);
        }
    }
}