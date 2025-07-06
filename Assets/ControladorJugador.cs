using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Configuración General")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 10f;

    // --- Variables de estado y componentes ---
    private bool estaEnSuelo = false;
    private Rigidbody2D rb;
    private Vector2 vectorDeMovimientoActual; // Este valor será proporcionado por el CharacterSwitcher

    private void Awake()
    {
        // Obtenemos la referencia al Rigidbody2D en el momento de la creación.
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate es el lugar correcto para toda la lógica de físicas.
    void FixedUpdate()
    {
        // Comprobamos el estado global del juego en cada ciclo de físicas.
        if (CharacterSwitcher.esModoBulletHellGlobal)
        {
            // Nos aseguramos de que la gravedad esté SIEMPRE desactivada en este modo.
            // La comprobación 'if (rb.gravityScale != 0)' evita estar asignando el valor en cada frame innecesariamente.
            if (rb.gravityScale != 0)
            {
                rb.gravityScale = 0;
            }
            ControlModoBulletHell();
        }
        else
        {
            // Nos aseguramos de que la gravedad esté SIEMPRE activada en este modo.
            // Asumimos que la gravedad normal es 1. Puedes cambiar este valor si usas otro en tu proyecto.
            if (rb.gravityScale != 1)
            {
                rb.gravityScale = 3f;
            }
            ControlModoPlataforma();
        }
    }

    // --- MÉTODOS PÚBLICOS (API) ---
    // Estos métodos son llamados por el CharacterSwitcher para controlar al personaje.

    public void SetVectorDeMovimiento(Vector2 nuevoVector)
    {
        // El CharacterSwitcher actualiza el vector de movimiento deseado.
        vectorDeMovimientoActual = nuevoVector;
    }

    public void RealizarSalto()
    {
        // El CharacterSwitcher nos pide que saltemos.
        // La lógica interna decide si realmente es posible hacerlo.
        if (!CharacterSwitcher.esModoBulletHellGlobal && estaEnSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    // --- LÓGICA DE MOVIMIENTO INTERNA ---

    private void ControlModoPlataforma()
    {
        // En modo plataforma, solo controlamos el eje X.
        // El eje Y se deja intacto para que la gravedad y la fuerza del salto actúen sobre él.
        rb.linearVelocity = new Vector2(vectorDeMovimientoActual.x * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void ControlModoBulletHell()
    {
        // En modo bullet hell, controlamos AMBOS ejes (X e Y).
        // Esto anula cualquier velocidad vertical previa (como la causada por la gravedad residual)
        // y proporciona un control total del movimiento.
        rb.linearVelocity = vectorDeMovimientoActual.normalized * velocidadMovimiento;
    }

    // --- DETECCIÓN DE COLISIONES ---

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