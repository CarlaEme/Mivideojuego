using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float rapidez = 5f;
    public float fuerzaSalto = 10f;

    [Header("Deteccion de Suelo")]
    public LayerMask capaSuelo; 
    public float radioDeteccion = 0.2f; 
    public Transform controladorSuelo; 

    [Header("Referencias")]
    public Rigidbody2D rb;
    public Animator animator;
    public MecanicaDash scriptDash; 
    public SpriteRenderer spriteJugador; 

    [Header("Configuración de Agachado")]
    public BoxCollider2D colisionadorJugador; 
    public float factorReduccionAltura = 0.5f; 
    [Range(0.1f, 1f)] public float escalaVisualY = 0.6f; 

    private float direccionX;
    private bool enSuelo; 
    private bool estaAgachado = false;

    private Vector2 tamañoOriginalCol;
    private Vector2 offsetOriginalCol;
    private Vector3 escalaOriginalTransform;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteJugador == null) spriteJugador = GetComponent<SpriteRenderer>();

        if (colisionadorJugador != null)
        {
            tamañoOriginalCol = colisionadorJugador.size;
            offsetOriginalCol = colisionadorJugador.offset;
        }
        escalaOriginalTransform = transform.localScale;
    }

    void Update()
    {
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        // Lectura de movimiento y detección de suelo
        direccionX = Input.GetAxisRaw("Horizontal");
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, capaSuelo);

        // --- LÓGICA DE AGACHADO ---
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            IntentarAgacharse();
        }
        else
        {
            IntentarLevantarse();
        }

        // --- SECCIÓN DE ANIMACIONES ---
        if (animator != null)
        {
            // Controla caminar/correr
            animator.SetFloat("Velocidad", Mathf.Abs(direccionX));
            
            // CAMBIO 1: Controla el estado de salto/caída automáticamente
            // Si NO está en el suelo, activa la animación. Si toca suelo, la apaga.
            animator.SetBool("estaSaltando", !enSuelo);
        }

        // Salto (No permitido si está agachado)
        if (Input.GetButtonDown("Jump") && enSuelo && !estaAgachado) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            
            // CAMBIO 2 (Opcional pero recomendado): Forzar el cambio visual inmediato al saltar
            if (animator != null) animator.SetBool("estaSaltando", true);
        }

        // Voltear el personaje
        if (direccionX > 0) transform.localScale = new Vector3(escalaOriginalTransform.x, transform.localScale.y, escalaOriginalTransform.z);
        else if (direccionX < 0) transform.localScale = new Vector3(-escalaOriginalTransform.x, transform.localScale.y, escalaOriginalTransform.z);

        if (transform.position.y < -10f) 
        {
            Reaparecer();
        }
    }

    void Reaparecer()
    {
        transform.position = new Vector3(0, 0, 0); 
        if (rb != null) 
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        float velocidadFinal = estaAgachado ? rapidez * 0.5f : rapidez;
        rb.linearVelocity = new Vector2(direccionX * velocidadFinal, rb.linearVelocity.y);
    }

    void IntentarAgacharse()
    {
        if (estaAgachado) return;
        estaAgachado = true;

        float nuevaAlturaY = tamañoOriginalCol.y * factorReduccionAltura;
        float diferencia = tamañoOriginalCol.y - nuevaAlturaY;
        colisionadorJugador.size = new Vector2(tamañoOriginalCol.x, nuevaAlturaY);
        colisionadorJugador.offset = new Vector2(offsetOriginalCol.x, offsetOriginalCol.y - (diferencia / 2f));

        transform.localScale = new Vector3(transform.localScale.x, escalaOriginalTransform.y * escalaVisualY, escalaOriginalTransform.z);

        if (animator != null) animator.SetBool("Agachado", true);
    }

    void IntentarLevantarse()
    {
        if (!estaAgachado) return;
        estaAgachado = false;

        colisionadorJugador.size = tamañoOriginalCol;
        colisionadorJugador.offset = offsetOriginalCol;

        transform.localScale = new Vector3(transform.localScale.x, escalaOriginalTransform.y, escalaOriginalTransform.z);

        if (animator != null) animator.SetBool("Agachado", false);
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioDeteccion);
        }
    }
}