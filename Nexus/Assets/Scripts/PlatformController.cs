using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float timeToDisappear = 2f; // Tiempo en segundos antes de que la plataforma desaparezca
    private Rigidbody rb;

    void OnTriggerStay(Collider other)
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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Hace que la plataforma sea cinemática para que no se vea afectada por la física
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX; // Congela el movimiento en los ejes X y Z para que la plataforma solo se mueva verticalmente
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
