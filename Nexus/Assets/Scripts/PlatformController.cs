using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Tiempo en segundos antes de que la plataforma comience a caer
    [SerializeField] private float timeToDisappear = 2f; // Tiempo en segundos antes de que la plataforma desaparezca
    [SerializeField] private bool fallsDown = true; // Indica si la plataforma ya ha comenzado a caer

    private Rigidbody rb;
    private bool isActivated = false; // Indica si la plataforma ha sido activada por el jugador

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Hace que la plataforma sea cinemática para que no se vea afectada por la física
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Congela la rotación para evitar que la plataforma gire al caer
       // rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX; // Congela el movimiento en los ejes X y Z para que la plataforma solo se mueva verticalmente
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(CrumbleSequence()); // Inicia la secuencia de desmoronamiento de la plataforma
        }
    }

    IEnumerator CrumbleSequence()
    {
        yield return new WaitForSeconds(fallDelay); // Espera el tiempo antes de que la plataforma comience a caer

        if(fallsDown)
            {
            rb.isKinematic = false; // Permite que la plataforma sea afectada por la física para que caiga
            rb.useGravity = true; // Activa la gravedad para que la plataforma caiga
            }

        yield return new WaitForSeconds(timeToDisappear); // Espera el tiempo antes de que la plataforma desaparezca
        Destroy(gameObject); // Destruye la plataforma
    }

    /*void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;
            Vector3 platformPosition = transform.position;
                if (playerPosition.y > platformPosition.y) // Verifica si el jugador está por encima de la plataforma
                {
                    rb.isKinematic = false; // Permite que la plataforma sea afectada por la física para que caiga
                    Destroy(gameObject, timeToDisappear); // Destruye la plataforma después del tiempo especificado
                Debug.Log("Player está sobre la plataforma");
            }
        }
    }*/
}
