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
    public MecanicaDash scriptDash; // Referencia al script del Dash
    public SpriteRenderer spriteJugador; // Arrastra el SpriteRenderer aquí

    [Header("Configuración de Agachado")]
    public BoxCollider2D colisionadorJugador; // Arrastra el BoxCollider2D del Player aquí
    public float factorReduccionAltura = 0.5f; // Reduce la colisión al 50%
    [Range(0.1f, 1f)] public float escalaVisualY = 0.6f; // Qué tanto se "aplasta" el dibujo

    private float direccionX;
    private bool enSuelo; 
    private bool estaAgachado = false;

    // Variables para guardar valores originales
    private Vector2 tamañoOriginalCol;
    private Vector2 offsetOriginalCol;
    private Vector3 escalaOriginalTransform;

    void Start()
    {
        // Si no asignaste el RB por inspector, lo buscamos
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteJugador == null) spriteJugador = GetComponent<SpriteRenderer>();

        // Guardamos los valores iniciales para poder restaurarlos
        if (colisionadorJugador != null)
        {
            tamañoOriginalCol = colisionadorJugador.size;
            offsetOriginalCol = colisionadorJugador.offset;
        }
        escalaOriginalTransform = transform.localScale;
    }

    void Update()
    {
        // Si el dash está activo, no hacemos nada más
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        // Lectura de movimiento
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

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(direccionX));
        }

        // Salto (No permitido si está agachado)
        if (Input.GetButtonDown("Jump") && enSuelo && !estaAgachado) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // Voltear el personaje
        if (direccionX > 0) transform.localScale = new Vector3(escalaOriginalTransform.x, transform.localScale.y, escalaOriginalTransform.z);
        else if (direccionX < 0) transform.localScale = new Vector3(-escalaOriginalTransform.x, transform.localScale.y, escalaOriginalTransform.z);

        // Si la posición Y del personaje es menor a -10 (caída al vacío)
    if (transform.position.y < -10f) 
    {
        Reaparecer();
    }
}

void Reaparecer()
{
    // Opción A: Mandarlo a una posición segura (ejemplo: x=0, y=0)
    transform.position = new Vector3(0, 0, 0); 
    
    // IMPORTANTE: Quitamos la velocidad acumulada de la caída para que no aparezca "disparado"
    if (rb != null) 
    {
        rb.linearVelocity = Vector2.zero;
    }

    }

    void FixedUpdate()
    {
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        // Caminar más lento si está agachado
        float velocidadFinal = estaAgachado ? rapidez * 0.5f : rapidez;
        rb.linearVelocity = new Vector2(direccionX * velocidadFinal, rb.linearVelocity.y);
    }

    void IntentarAgacharse()
    {
        if (estaAgachado) return;
        estaAgachado = true;

        // 1. Reducir Colisionador (Física)
        float nuevaAlturaY = tamañoOriginalCol.y * factorReduccionAltura;
        float diferencia = tamañoOriginalCol.y - nuevaAlturaY;
        colisionadorJugador.size = new Vector2(tamañoOriginalCol.x, nuevaAlturaY);
        colisionadorJugador.offset = new Vector2(offsetOriginalCol.x, offsetOriginalCol.y - (diferencia / 2f));

        // 2. Aplastar Sprite (Visual)
        transform.localScale = new Vector3(transform.localScale.x, escalaOriginalTransform.y * escalaVisualY, escalaOriginalTransform.z);

        if (animator != null) animator.SetBool("Agachado", true);
    }

    void IntentarLevantarse()
    {
        if (!estaAgachado) return;
        estaAgachado = false;

        // 1. Restaurar Colisionador
        colisionadorJugador.size = tamañoOriginalCol;
        colisionadorJugador.offset = offsetOriginalCol;

        // 2. Restaurar Escala Visual
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