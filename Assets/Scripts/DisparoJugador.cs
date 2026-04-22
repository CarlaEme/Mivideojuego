using UnityEngine;
using UnityEngine.EventSystems; // 1. IMPORTANTE: Añade esta línea para detectar la UI

public class DisparoJugador : MonoBehaviour
{
    [Header("Configuracion de Disparo")]
    public Transform controladorDisparo; 
    public GameObject balaPrefab;        
    public float velocidadBala = 20f;

   
   void Update()
{
    if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.F))
    {
        // Añadimos una verificación extra: Solo bloquea si el EventSystem existe
        if (EventSystem.current != null) 
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clic bloqueado por la Interfaz");
                return; 
            }
        }

        Disparar();
    }
}
   
   
   

    void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, controladorDisparo.position, controladorDisparo.rotation);
        
        float sentidoX = transform.localScale.x;
        float tamañoDeseado = 0.4f; 
        bala.transform.localScale = new Vector3(tamañoDeseado * sentidoX, tamañoDeseado, 1f);

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(sentidoX * velocidadBala, rb.linearVelocity.y);
        
        Destroy(bala, 2f);
    }
}


