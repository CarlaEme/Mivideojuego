using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [Header("Configuracion de Disparo")]
    public Transform controladorDisparo; // El punto que creamos
    public GameObject balaPrefab;        // El molde de la bala
    public float velocidadBala = 20f;

    void Update()
    {
        // Detectar evento de teclado (Fire1 = Clic izq o Ctrl izq, o tecla F)
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.F))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, controladorDisparo.position, controladorDisparo.rotation);
        
        // --- SOLUCIÓN PARA EL TAMAÑO ---
        // Obtenemos el sentido del personaje (1 o -1)
        float sentidoX = transform.localScale.x;

        // Aquí definimos el tamaño exacto que queremos (ejemplo: 0.2)
        // Multiplicamos el tamaño por el sentido para que se voltee
        float tamañoDeseado = 0.4f; 
        bala.transform.localScale = new Vector3(tamañoDeseado * sentidoX, tamañoDeseado, 1f);
        // -------------------------------

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(sentidoX * velocidadBala, rb.linearVelocity.y);
        
        Destroy(bala, 2f);
    }
}