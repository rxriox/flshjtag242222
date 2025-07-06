using UnityEngine;

public class ActivadorModo : MonoBehaviour
{
    // OnTriggerEnter2D se llama automáticamente por el motor de físicas de Unity
    // cuando otro objeto con un Collider2D (y un Rigidbody2D) entra en este trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Primero, comprobamos si el objeto que ha entrado es realmente el jugador.
        // Usamos la etiqueta "Player" que deberíamos haber asignado a nuestros prefabs de personaje.
        if (other.CompareTag("Player"))
        {
            // Si es el jugador, llamamos al método estático del CharacterSwitcher.
            // Esto cambia la variable 'esModoBulletHellGlobal' a 'true'.
            CharacterSwitcher.ActivarModoBulletHellGlobal();

            // Para que este punto de cambio solo funcione una vez, lo destruimos
            // inmediatamente después de que ha cumplido su función.
            Destroy(gameObject);
        }
    }
}