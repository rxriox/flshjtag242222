using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Le damos una velocidad constante hacia arriba (en su eje Y local)
        rb.linearVelocity = transform.up * velocidad;

        // Le decimos que se destruya después de 'tiempoDeVida' segundos.
        Destroy(gameObject, tiempoDeVida);
    }

    // En el futuro, aquí podrías poner la lógica de colisión con enemigos
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ejemplo: if (other.CompareTag("Enemigo")) { Destroy(gameObject); }
    }
}