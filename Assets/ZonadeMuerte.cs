using UnityEngine;

public class ZonaDeMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Le decimos al GameManager que el jugador ha perdido una vida.
            // Usamos el singleton 'instance' para acceder a un método no estático.
            if (CharacterSwitcher.instance != null)
            {
                CharacterSwitcher.instance.PerderVida();
            }
        }
    }
}