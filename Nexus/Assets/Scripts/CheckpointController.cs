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
        }
    }
}
