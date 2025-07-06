using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Configuración General")]
    // Ahora tenemos dos velocidades separadas
    public float velocidadMovimientoPlataforma = 5f;
    public float velocidadMovimientoBulletHell = 8f; // Más rápida por defecto
    public float fuerzaSalto = 10f;

    private bool estaEnSuelo = false;
    private Rigidbody2D rb;
    private Vector2 vectorDeMovimientoActual;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (CharacterSwitcher.esModoBulletHellGlobal)
        {
            if (rb.gravityScale != 0) {
                rb.gravityScale = 0;
            }
            ControlModoBulletHell();
        }
        else
        {
            if (rb.gravityScale != 1) {
                rb.gravityScale = 1;
            }
            ControlModoPlataforma();
        }
    }

    public void SetVectorDeMovimiento(Vector2 nuevoVector)
    {
        vectorDeMovimientoActual = nuevoVector;
    }

    public void RealizarSalto()
    {
        if (!CharacterSwitcher.esModoBulletHellGlobal && estaEnSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    private void ControlModoPlataforma()
    {
        // Usa la velocidad específica para el modo plataforma
        rb.linearVelocity = new Vector2(vectorDeMovimientoActual.x * velocidadMovimientoPlataforma, rb.linearVelocity.y);
    }

    private void ControlModoBulletHell()
    {
        // Usa la velocidad específica para el modo bullet hell
        rb.linearVelocity = vectorDeMovimientoActual.normalized * velocidadMovimientoBulletHell;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
        }
    }
}