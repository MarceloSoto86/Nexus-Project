using UnityEngine;

public class KillZone : MonoBehaviour
{
    //public PlayerStatus _playerStatus; // Referencia al componente PlayerStatus del jugador
    //public GameObject player;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if( PlayerController.player != null) // Verifica que la referencia al jugador no sea nula antes de intentar acceder a ella
            {
                PlayerController.player.Respawn(); // Llama al método Teleport del jugador para teletransportarlo al checkpoint
                PlayerStatus _status = PlayerController.player.GetComponent<PlayerStatus>();
                if(_status != null) // Verifica que el componente PlayerStatus no sea nulo antes de intentar acceder a él_status
                { 
                _status.ResetStatus(); // Llama al método ResetStatus del jugador para restablecer su estado
                }
            }

        }
    }
}
