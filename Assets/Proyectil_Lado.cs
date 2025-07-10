using UnityEngine;

public class Proyectil_Lado : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 3f;

    void Start()
    {
        // Se mueve hacia su 'derecha' local. El jugador lo rotará si dispara a la izquierda.
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;
        Destroy(gameObject, tiempoDeVida);
    }
}