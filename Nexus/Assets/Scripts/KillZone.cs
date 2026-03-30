using UnityEngine;

public class KillZone : MonoBehaviour
{
    //public GameObject player;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if( PlayerController.player != null) // Verifica que la referencia al jugador no sea nula antes de intentar acceder a ella
            {
                PlayerController.player.Respawn(); // Llama al método Teleport del jugador para teletransportarlo al checkpoint
            }

        }
    }
}
