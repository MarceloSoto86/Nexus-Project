using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Tiempo en segundos antes de que la plataforma comience a caer
    [SerializeField] private float timeToDisappear = 2f; // Tiempo en segundos antes de que la plataforma desaparezca
    [SerializeField] private float reappearDelay = 3f; // Tiempo en segundos antes de que la plataforma reaparezca después de desaparecer
    [SerializeField] private bool fallsDown = true; // Indica si la plataforma ya ha comenzado a caer
    public MeshRenderer _meshRenderer; // Referencia al componente MeshRenderer para controlar la visibilidad de la plataforma
    public Collider platformCollider; // Referencia al componente Collider para controlar la colisión de la plataforma
   
    private MovingPlatform mp; // Referencia al script de movimiento de la plataforma, si existe
    private Rigidbody rb;
    private Vector3 initialPosition; // Almacena la posición inicial de la plataforma para restablecerla después de reaparecer
    private Quaternion initialRotation; // Almacena la rotación inicial de la plataforma para restablecerla después de reaparecer
    private bool isActivated = false; // Indica si la plataforma ha sido activada por el jugador
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position; // Guarda la posición inicial de la plataforma
        initialRotation = transform.rotation; // Guarda la rotación inicial de la plataforma
        _meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Hace que la plataforma sea cinemática para que no se vea afectada por la física
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Congela la rotación para evitar que la plataforma gire al caer
       // rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX; // Congela el movimiento en los ejes X y Z para que la plataforma solo se mueva verticalmente
       mp = GetComponent<MovingPlatform>();
    }

    // Detecta cuando el jugador entra en contacto con la plataforma para iniciar la secuencia de desmoronamiento
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(CrumbleSequence()); // Inicia la secuencia de desmoronamiento de la plataforma
        }
    }
    // Secuencia para hacer que la plataforma se desmorone y desaparezca después de un tiempo
    IEnumerator CrumbleSequence()
    {
        yield return new WaitForSeconds(fallDelay); // Espera el tiempo antes de que la plataforma comience a caer

        if(fallsDown)
            {
            rb.isKinematic = false; // Permite que la plataforma sea afectada por la física para que caiga
            rb.useGravity = true; // Activa la gravedad para que la plataforma caiga
            mp = GetComponent<MovingPlatform>();
            if (mp != null) mp.enabled = false; // Al empezar a caer
        }

        yield return new WaitForSeconds(timeToDisappear); // Espera el tiempo antes de que la plataforma desaparezca
        if (_meshRenderer != null)
        {
            _meshRenderer.enabled = false;
        }
        else
        {
            Debug.LogWarning("Ojo: No hay MeshRenderer en " + gameObject.name + ", pero la secuencia sigue.");
        }
        if (platformCollider != null)
        {
            platformCollider.enabled = false; // Desactiva el collider para que el jugador pueda pasar a través de la plataforma
        }
        else
        {
            Debug.LogWarning("Ojo: No hay Collider en " + gameObject.name + ", pero la secuencia sigue.");
        }
        StartCoroutine(ReappearSequence()); // Inicia la secuencia de reaparecer la plataforma
        //Destroy(gameObject); // Destruye la plataforma
    }
    // Secuencia para hacer que la plataforma reaparezca después de un tiempo
    IEnumerator ReappearSequence()
    { yield return new WaitForSeconds(reappearDelay);

        transform.position = initialPosition; // Restablece la posición inicial de la plataforma
        transform.rotation = initialRotation; // Restablece la rotación inicial de la plataforma
        //rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;     // Detiene el movimiento lineal
            rb.angularVelocity = Vector3.zero;    // Detiene cualquier rotación
            rb.isKinematic = true;                // La hace "inmóvil" temporalmente
        }
        if (_meshRenderer != null)
        {
            
            _meshRenderer.enabled = true; // Hace visible la plataforma nuevamente
        }
        else
        {
            Debug.LogWarning("Ojo: No hay MeshRenderer en " + gameObject.name + ", pero la secuencia sigue.");
        }
        if (platformCollider != null)
        {
            platformCollider.enabled = true; // Activa el collider para que el jugador pueda interactuar con la plataforma nuevamente
        }
        else
        {
            Debug.LogWarning("Ojo: No hay Collider en " + gameObject.name + ", pero la secuencia sigue.");
        }
        if (mp != null) mp.enabled = true;  // Al estar ya en la posición inicial
        isActivated = false; // Restablece el estado de activación para permitir que la plataforma se active nuevamente
    }
  
}
