using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CharacterSwitcher : MonoBehaviour
{
    public static CharacterSwitcher instance;
    public static bool esModoBulletHellGlobal = false;

    [Header("Cámaras Virtuales")]
    public CinemachineCamera vcamPlataforma;
    public CinemachineCamera vcamBulletHell;

    [Header("Efecto de Cámara Inicial")]
    public float zoomInicial = 15f;
    public float zoomFinalPlataforma = 7f;
    public float duracionZoomInicial = 2f;

    [Header("Prefabs de Personajes")]
    public List<GameObject> prefabsPersonajes;

    [Header("Configuración de Recarga")]
    public float tiempoDeRecarga = 20f;
    private float recargaActual = 0f;

    [Header("UI (Opcional)")]
    public TextMeshProUGUI textoRecarga;

    private int indicePersonajeActivo = 0;
    private GameObject personajeActualInstanciado;
    private ControladorJugador controladorActual;
    private ControlesJugador controles;
    private Vector2 vectorDeMovimientoInput;

    public static void ActivarModoBulletHellGlobal()
    {
        esModoBulletHellGlobal = true;
        if (instance != null)
        {
            instance.CambiarACamaraBulletHell();
        }
    }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }

        controles = new ControlesJugador();
        controles.Plataformas.CambiarPersonaje.performed += _ => IntentarCambiarPersonaje();
        controles.Plataformas.Saltar.performed += _ => IntentarSalto();
        controles.Plataformas.Mover.performed += ctx => vectorDeMovimientoInput = ctx.ReadValue<Vector2>();
        controles.Plataformas.Mover.canceled += ctx => vectorDeMovimientoInput = Vector2.zero;
    }

    private void OnEnable() { controles.Plataformas.Enable(); }
    private void OnDisable() { controles.Plataformas.Disable(); }

    void Start()
    {
        esModoBulletHellGlobal = false;

        if (vcamPlataforma != null)
        {
            vcamPlataforma.gameObject.SetActive(true);
            vcamPlataforma.Priority = 10;
            vcamPlataforma.Lens.OrthographicSize = zoomInicial;
        }
        if (vcamBulletHell != null) {
            vcamBulletHell.gameObject.SetActive(false);
            vcamBulletHell.Priority = 5;
        }

        if (prefabsPersonajes.Count > 0)
        {
            CrearPersonaje(indicePersonajeActivo, transform.position, Quaternion.identity);
            StartCoroutine(AnimarZoomInicial());
        }
    }

    void Update()
    {
        if (controladorActual != null)
        {
            controladorActual.SetVectorDeMovimiento(vectorDeMovimientoInput);
        }

        if (recargaActual > 0)
        {
            recargaActual -= Time.deltaTime;
        }
        
        ActualizarUI();
    }
    
    private void IntentarSalto()
    {
        if (controladorActual != null)
        {
            controladorActual.RealizarSalto();
        }
    }

    private void IntentarCambiarPersonaje()
    {
        if (recargaActual <= 0 && personajeActualInstanciado != null)
        {
            CambiarPersonaje();
        }
    }
    
    void CambiarPersonaje()
    {
        Vector3 posicionActual = personajeActualInstanciado.transform.position;
        Quaternion rotacionActual = personajeActualInstanciado.transform.rotation;
        Destroy(personajeActualInstanciado);
        indicePersonajeActivo++;
        if (indicePersonajeActivo >= prefabsPersonajes.Count)
        {
            indicePersonajeActivo = 0;
        }
        CrearPersonaje(indicePersonajeActivo, posicionActual, rotacionActual);
        recargaActual = tiempoDeRecarga;
        Debug.Log("Cambiado a: " + prefabsPersonajes[indicePersonajeActivo].name);
    }
    
    void CrearPersonaje(int indice, Vector3 posicion, Quaternion rotacion)
    {
        personajeActualInstanciado = Instantiate(prefabsPersonajes[indice], posicion, rotacion);
        controladorActual = personajeActualInstanciado.GetComponent<ControladorJugador>();

        if (vcamPlataforma != null)
        {
            vcamPlataforma.Follow = personajeActualInstanciado.transform;
            vcamPlataforma.LookAt = personajeActualInstanciado.transform;
        }

        if (controladorActual == null)
        {
            Debug.LogError("El prefab " + personajeActualInstanciado.name + " no tiene el script ControladorJugador.");
        }
    }

    public void CambiarACamaraBulletHell()
    {
        if (vcamBulletHell != null)
        {
            vcamBulletHell.gameObject.SetActive(true);
            vcamBulletHell.Priority = 20;
        }
    }
    
    private IEnumerator AnimarZoomInicial()
    {
        float tiempoTranscurrido = 0f;
        float zoomActual = vcamPlataforma.Lens.OrthographicSize;

        while (tiempoTranscurrido < duracionZoomInicial)
        {
            vcamPlataforma.Lens.OrthographicSize = Mathf.Lerp(zoomActual, zoomFinalPlataforma, tiempoTranscurrido / duracionZoomInicial);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        vcamPlataforma.Lens.OrthographicSize = zoomFinalPlataforma;
        Debug.Log("Animación de zoom inicial completada.");
    }

    void ActualizarUI()
    {
        if (textoRecarga != null)
        {
            if (recargaActual > 0)
            {
                textoRecarga.text = "Cambio en: " + recargaActual.ToString("F1") + "s";
            }
            else
            {
                textoRecarga.text = "Cambio de personaje: LISTO (C)";
            }
        }
    }
}