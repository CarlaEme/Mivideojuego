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

    private float direccionX;
    private bool enSuelo; 

    void Update()
    {
        // Si el script de dash existe y el personaje está haciendo dash, salimos del Update
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        direccionX = Input.GetAxisRaw("Horizontal");
        
        // Detectamos si tocamos el suelo
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, capaSuelo);

        // Animación
        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(direccionX));
        }

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // Orientación del personaje (Voltear)
        if (direccionX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direccionX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        // Si estamos haciendo dash, NO aplicamos velocidad de caminata
        if (scriptDash != null && scriptDash.estaHaciendoDash) return;

        rb.linearVelocity = new Vector2(direccionX * rapidez, rb.linearVelocity.y);
    }

    // Dibuja el círculo de detección en el editor
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioDeteccion);
        }
    }
}