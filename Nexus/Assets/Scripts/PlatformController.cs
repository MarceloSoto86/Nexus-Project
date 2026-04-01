using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Tiempo en segundos antes de que la plataforma comience a caer
    [SerializeField] private float timeToDisappear = 2f; // Tiempo en segundos antes de que la plataforma desaparezca
    [SerializeField] private float reappearDelay = 3f; // Tiempo en segundos antes de que la plataforma reaparezca después de desaparecer
    [SerializeField] private bool fallsDown = true; // Indica si la plataforma ya ha comenzado a caer
    public MeshRenderer meshRenderer; // Referencia al componente MeshRenderer para controlar la visibilidad de la plataforma
    public Collider platformCollider; // Referencia al componente Collider para controlar la colisión de la plataforma
   

    private Rigidbody rb;
    private bool isActivated = false; // Indica si la plataforma ha sido activada por el jugador
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Hace que la plataforma sea cinemática para que no se vea afectada por la física
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Congela la rotación para evitar que la plataforma gire al caer
       // rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX; // Congela el movimiento en los ejes X y Z para que la plataforma solo se mueva verticalmente
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
            }

        yield return new WaitForSeconds(timeToDisappear); // Espera el tiempo antes de que la plataforma desaparezca
        meshRenderer.enabled = false; // Hace invisible la plataforma
        platformCollider.enabled = false; // Desactiva el collider para que el jugador pueda pasar a través de la plataforma
        StartCoroutine(ReappearSequence()); // Inicia la secuencia de reaparecer la plataforma
        //Destroy(gameObject); // Destruye la plataforma
    }
    // Secuencia para hacer que la plataforma reaparezca después de un tiempo
    IEnumerator ReappearSequence()
    { yield return new WaitForSeconds(reappearDelay);
       
        meshRenderer.enabled = true; // Hace visible la plataforma nuevamente
        platformCollider.enabled = true; // Activa el collider para que el jugador pueda interactuar con la plataforma nuevamente
        isActivated = false; // Restablece el estado de activación para permitir que la plataforma se active nuevamente
    }
  
}
