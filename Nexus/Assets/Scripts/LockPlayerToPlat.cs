using UnityEngine;

public class LockPlayerToPlat : MonoBehaviour
{
    private bool lockPlayer;
    public Transform player;
    private Vector3 offset;
    private Vector3 lastPlatformPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lockPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        PlayerLock();
        lastPlatformPosition = transform.position; // Actualiza la posición de la plataforma para el próximo cálculo
    }

    // Esta función bloquea el movimiento del jugador con la plataforma mientras esté en contacto con ella
    private void PlayerLock()
    {
        if (lockPlayer && player != null)
        {
            Debug.Log("Player movido con la plataforma");
            Vector3 platformMovement = transform.position - lastPlatformPosition; // Calcula el movimiento de la plataforma desde el último frame
            player.position += platformMovement; // Mueve al jugador junto con la plataforma
            
        }
    }

    // Estas funciones detectan cuando el jugador entra o sale de la plataforma para activar o desactivar el bloqueo
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player en contacto con la plataforma");
       
        if (!lockPlayer)
            {
                lastPlatformPosition = transform.position; // Inicializa la posición de la plataforma cuando el jugador entra en contacto
        
                player = other.transform;
                lockPlayer = true;
            }
        }
    }
    //Esta función se asegura de que el jugador deje de moverse con la plataforma cuando salga de ella
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player salió de la plataforma");
           // collision.transform.SetParent(null);
           // collision.transform.localScale = Vector3.one;
            lockPlayer = false;
            player = null;
        }
    }
}
