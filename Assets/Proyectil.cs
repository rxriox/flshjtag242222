using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float distanciaMaxima = 15f;

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición donde se creó.
        posicionInicial = transform.position;
        // Impulsamos el proyectil hacia su derecha local (la dirección a la que apunta).
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;
    }

    void Update()
    {
        // Si ha viajado más de la distancia máxima, se destruye.
        if (Vector3.Distance(posicionInicial, transform.position) > distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con un enemigo...
        if (other.CompareTag("Enemigo"))
        {
            // Le decimos al enemigo que ha recibido daño.
            other.GetComponent<Enemigo>().RecibirDano();
            // Y destruimos el proyectil.
            Destroy(gameObject);
        }
    }
}