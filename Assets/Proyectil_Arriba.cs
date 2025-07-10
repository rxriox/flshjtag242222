using UnityEngine;

public class Proyectil_Arriba : MonoBehaviour
{
    public float velocidad = 25f; // O el valor que le hayas puesto
    public float tiempoDeVida = 3f;

    void Start()
    {
        // Esta línea usa Vector2.up, que es la dirección (0, 1) en el espacio del mundo.
        // Siempre irá hacia arriba, sin importar la rotación del objeto.
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * velocidad;

        Destroy(gameObject, tiempoDeVida);
    }
}