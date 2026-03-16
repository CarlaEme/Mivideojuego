using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuracion de Movimiento")]
    public float rapidez = 5f;
    public float fuerzaSalto = 10f;

    [Header("Deteccion de Suelo")]
    public LayerMask capaSuelo; // Para seleccionar la capa "Suelo"
    public float radioDeteccion = 0.2f; // Tamaño del circulo de deteccion
    public Transform controladorSuelo; // Un punto en los pies del personaje

    [Header("Referencias")]
    public Rigidbody2D rb;
    public Animator animator;

    private float direccionX;
    private bool enSuelo; // Nos dira si estamos tocando el piso

    void Update()
    {
        direccionX = Input.GetAxisRaw("Horizontal");

        // Creamos un circulo invisible en los pies para detectar el suelo
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, capaSuelo);

        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(direccionX));
        }

        // SALTO: Solo funciona si presionas el boton Y estas en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        if (direccionX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direccionX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direccionX * rapidez, rb.linearVelocity.y);
    }

    // Para ver el circulo de deteccion en el editor (ayuda mucho)
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioDeteccion);
        }
    }
}