using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void SalirJuego()
    {
        GameManager.instance.SalirDelJuego();
    }
}