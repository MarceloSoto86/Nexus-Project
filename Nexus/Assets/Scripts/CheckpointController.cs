using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public bool isSecurityActive = false; // Variable para controlar si el sistema de seguridad está activo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isSecurityActive)
            {
                SecuritySystemActivation.TriggerSecuritySystem();
                isSecurityActive = true; // Activa el sistema de seguridad para evitar que se active nuevamente
                Debug.Log("Security system activated at checkpoint: " + transform.position); // Imprime un mensaje de depuración indicando que el sistema de seguridad ha sido activado
            }
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
