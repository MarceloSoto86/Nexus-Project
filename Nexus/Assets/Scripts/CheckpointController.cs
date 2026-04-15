using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetCheckpoint(transform.position); // Establece la posición del checkpoint al jugador
                Debug.Log("Checkpoint reached at: " + transform.position); // Imprime un mensaje de depuración con la posición del checkpoint alcanzado
            }

            //Buscamos el componente Player Events para activar el evento de checkpoint alcanzado
            var playerEvents = other.GetComponent<PlayerEvents>();
            if(playerEvents != null)
            {
                playerEvents.TriggerCheckpoint(transform.position.y); // Dispara el evento de checkpoint alcanzado con la altura del checkpoint
            }
            //Desactiva el checkpoint para que no pueda ser alcanzado nuevamente
            gameObject.SetActive(false);
        }
    }
}
