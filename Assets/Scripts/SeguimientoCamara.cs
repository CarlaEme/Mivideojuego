using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo; // Aquí arrastraremos al Player
    public float suavizado = 0.125f; // Qué tan "elástica" es la cámara
    public Vector3 desfase; // Distancia entre la cámara y el jugador

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Calculamos la posición deseada (Posición del player + el desfase)
        Vector3 posicionDeseada = objetivo.position + desfase;
        
        // Interpolamos entre la posición actual y la deseada para suavizar
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        
        // Aplicamos la posición a la cámara
        transform.position = posicionSuavizada;
    }
}