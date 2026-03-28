using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [Header("Configuración del Prefab")]
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("Ajustes de Tiempo")]
    public float fireRate = 0.5f; // Tiempo de espera (medio segundo)
    private float nextFireTime = 0f;

    [Header("Controles")]
    public KeyCode fireKey = KeyCode.Alpha2;

    void Update()
    {
        // Usamos GetKey para que pueda disparar seguido si dejas presionado
        if (Input.GetKey(fireKey) && Time.time >= nextFireTime)
        {
            FireSingleMissile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireSingleMissile()
    {
        if (missilePrefab == null || firePoint == null) return;

        // Detectar hacia dónde mira el padre (localScale.x)
        float lookDirection = firePoint.parent.localScale.x;
        Vector2 fireDirection = (lookDirection > 0) ? Vector2.right : Vector2.left;

        // Instanciar un único misil en la posición del FirePoint
        GameObject newMissile = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = newMissile.GetComponent<Rigidbody2D>();
        Missile missileScript = newMissile.GetComponent<Missile>();

        if (rb != null && missileScript != null)
        {
            // Aplicar velocidad lineal
            rb.linearVelocity = fireDirection * missileScript.speed;
        }

        SpriteRenderer rend = newMissile.GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            // FlipX: True si mira a la derecha (porque tu dibujo original ve a la izquierda)
            rend.flipX = (lookDirection > 0);
        }

        Debug.Log("Misil único disparado correctamente.");
    }
}