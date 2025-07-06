using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public void RecibirDano()
    {
        Debug.Log(gameObject.name + " ha recibido daño!");
        // Por ahora, simplemente lo destruimos.
        // Más adelante aquí podrías restar vida, etc.
        Destroy(gameObject);
    }
}