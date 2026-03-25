using UnityEngine;

public class Bala : MonoBehaviour
{
    // Seguro de vida: la bala desaparecerá en 3 segundos si no choca con nada
    public float tiempoDeVida = 3f;

    void Start()
    {
        // Esto le dice a Unity: "Pase lo que pase, borra este objeto en 3 segundos"
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la bala toca cualquier cosa que no sea el jugador...
        if (!collision.CompareTag("Player"))
        {
            // ¡Pum! Se destruye inmediatamente al chocar
            Destroy(gameObject);
        }
    }
}