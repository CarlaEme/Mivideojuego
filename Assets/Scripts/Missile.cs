using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;

    void Start()
    {
        // El misil se destruirá solo después de lifeTime segundos
        Destroy(gameObject, lifeTime);
    }
    
    // IMPORTANTE: Borra cualquier cosa que diga Update o FixedUpdate aquí.
    // Si hay movimiento aquí, ignorará lo que diga el MissileLauncher.
}