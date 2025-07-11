using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Configuración General")]
    public float velocidadMovimientoPlataforma = 5f;
    public float velocidadMovimientoBulletHell = 8f;
    public float fuerzaSalto = 10f;

    [Header("Prefabs de Proyectil")]
    public GameObject proyectilPlataformaPrefab; // Para el disparo lateral
    public GameObject proyectilBulletHellPrefab; // Para el disparo hacia arriba

    [Header("Puntos de Disparo")]
    public Transform puntoDisparoPlataforma; // Disparo lateral
    public Transform puntoDisparoBulletHell; // disparo en modo Bullet Hell

    [Header("Disparo en Plataforma")]
    public float recargaDisparoPlataforma = 2f;
    private float temporizadorDisparoPlataforma = 0f;

    // Variables Internas
    private bool estaEnSuelo = false;
    private Rigidbody2D rb;
    private Vector2 vectorDeMovimientoActual;
    private int direccionMirada = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (temporizadorDisparoPlataforma > 0)
        {
            temporizadorDisparoPlataforma -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (CharacterSwitcher.esModoBulletHellGlobal)
        {
            if (rb.gravityScale != 0) { rb.gravityScale = 0; }
            ControlModoBulletHell();
        }
        else
        {
            if (rb.gravityScale != 1) { rb.gravityScale = 1; }
            ControlModoPlataforma();
        }
    }

    public void SetVectorDeMovimiento(Vector2 nuevoVector)
    {
        vectorDeMovimientoActual = nuevoVector;
        if (Mathf.Abs(nuevoVector.x) > 0.1f)
        {
            direccionMirada = (int)Mathf.Sign(nuevoVector.x);
            transform.localScale = new Vector3(direccionMirada, 1, 1);
        }
    }

    public void RealizarDisparoPlataforma()
    {
        if (CharacterSwitcher.esModoBulletHellGlobal || temporizadorDisparoPlataforma > 0) return;

        if (proyectilPlataformaPrefab == null || puntoDisparoPlataforma == null)
        {
            Debug.LogError("Falta configurar 'Proyectil Plataforma Prefab' o 'Punto Disparo Plataforma' en el Inspector del jugador.");
            return;
        }

        Debug.Log("Disparo de plataforma realizado.");
        temporizadorDisparoPlataforma = recargaDisparoPlataforma;
        Quaternion rotacionProyectil = (direccionMirada > 0) ? puntoDisparoPlataforma.rotation : Quaternion.Euler(0, 180, 0);
        Instantiate(proyectilPlataformaPrefab, puntoDisparoPlataforma.position, rotacionProyectil);
    }

    public void RealizarDisparoBulletHell()
    {
        if (!CharacterSwitcher.esModoBulletHellGlobal) return;

        if (proyectilBulletHellPrefab == null || puntoDisparoBulletHell == null)
        {
            Debug.LogError("Falta configurar 'Proyectil Bullet Hell Prefab' o 'Punto Disparo Bullet Hell' en el Inspector del jugador.");
            return;
        }

        Debug.Log("Disparo en modo Bullet Hell realizado.");
        Instantiate(proyectilBulletHellPrefab, puntoDisparoBulletHell.position, puntoDisparoBulletHell.rotation);
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
        rb.linearVelocity = new Vector2(vectorDeMovimientoActual.x * velocidadMovimientoPlataforma, rb.linearVelocity.y);
    }

    private void ControlModoBulletHell()
    {
        rb.linearVelocity = vectorDeMovimientoActual.normalized * velocidadMovimientoBulletHell;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) estaEnSuelo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) estaEnSuelo = false;
    }
}
