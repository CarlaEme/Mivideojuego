using UnityEngine;
using System.Collections;

public class MecanicaDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite; 
    
    [Header("Configuración de Dash")]
    public float fuerzaDash = 30f; 
    public float tiempoDash = 0.2f;
    public float esperaEntreDashes = 1f;

    private bool puedeHacerDash = true;
    public bool estaHaciendoDash;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        if (estaHaciendoDash) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && puedeHacerDash)
        {
            StartCoroutine(EjecutarDash());
        }
    }

    private IEnumerator EjecutarDash()
    {
        puedeHacerDash = false;
        estaHaciendoDash = true;

        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0f; // Pausamos gravedad
        
        // Ponemos el color azul
       // sprite.color = new Color(0, 0.5f, 1f, 0.5f);

        // Determinamos dirección
        float direccion = transform.localScale.x;
        if (Mathf.Abs(direccion) < 0.1f) direccion = 1f;

        // --- MOVIMIENTO FORZADO ---
        // Aplicamos la velocidad directamente cada frame durante el tiempo del dash
        float tiempoPasado = 0;
        while (tiempoPasado < tiempoDash)
        {
            rb.linearVelocity = new Vector2(direccion * fuerzaDash, 0f);
            tiempoPasado += Time.deltaTime;
            yield return null; 
        }

        // Al terminar, frenamos un poco para que no salga volando infinitamente
        rb.linearVelocity = Vector2.zero;

        // Volvemos a la normalidad
        sprite.color = Color.white;
        rb.gravityScale = gravedadOriginal;
        estaHaciendoDash = false;

        yield return new WaitForSeconds(esperaEntreDashes);
        puedeHacerDash = true;
    }
}