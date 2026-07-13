using UnityEngine;

public class DynamicKillZone : MonoBehaviour
{
    private float stepHeight;

    private void Start()
    {
        //stepHeight = DifficultyManager.Instance.GetStepHeight(); // Obtiene la altura del paso desde el DifficultyManager
    }

    /*   public void HandleReachedHeight(float newHeight)
       {
           if (DifficultyManager.Instance == null) // Verifica si el DifficultyManager está disponible antes de intentar acceder a él
           {
               //Debug.logWarning("DifficultyManager no encontrado. Asegúrate de que el DifficultyManager esté presente en la escena.");
               return; // Sale del método si el DifficultyManager no está disponible para evitar errores
           }
           float step = DifficultyManager.Instance.GetStepHeight(); // Obtiene la altura del paso desde el DifficultyManager

           //Calcula la nueva posición del KillZone en función de la altura alcanzada por el jugador y la altura del paso
           float targetHeight = newHeight - step; // Calcula la altura objetivo restando la altura del paso a la nueva altura alcanzada por el jugador

           //Subimos el KillZone a la nueva altura objetivo
           if (targetHeight > transform.position.y) // Verifica si la altura objetivo es mayor que la altura actual del KillZone antes de moverlo
           {
               Vector3 newPosition = new Vector3(transform.position.x, targetHeight, transform.position.z); // Crea una nueva posición para el KillZone con la altura objetivo
               transform.position = newPosition; // Actualiza la posición del KillZone a la nueva posición calculada
           }
       }*/

    /*private void OnEnable()
    {
        PlayerEvents.OnPlayerHeightReached += HandleReachedHeight; // Suscribirse al evento de altura alcanzada por el jugador
    }
    private void OnDisable()
    {
        PlayerEvents.OnPlayerHeightReached -= HandleReachedHeight; // Desuscribirse del evento de altura alcanzada por el jugador
    }*/

    public void HandleCheckpointReached(float checkpointHeight)
    {
        float step = DifficultyManager.Instance.GetStepHeight(); // Obtiene la altura del paso desde el DifficultyManager
        //La KillZone se moverá a la altura del checkpoint alcanzado por el jugador menos la altura del paso, asegurando que el jugador siempre tenga un margen de seguridad por debajo del checkpoint alcanzado
        float targetHeight = checkpointHeight - step; // Calcula la altura objetivo restando la altura del paso a la altura del checkpoint alcanzado por el jugador

        if(targetHeight > transform.position.y) // Verifica si la altura objetivo es mayor que la altura actual del KillZone antes de moverlo
        {
            Vector3 newPosition = new Vector3(transform.position.x, targetHeight, transform.position.z); // Crea una nueva posición para el KillZone con la altura objetivo
            transform.position = newPosition; // Actualiza la posición del KillZone a la nueva posición calculada
        }
    }

    private void OnEnable()
    {
        PlayerEvents.OnCheckpointReached += HandleCheckpointReached; // Suscribirse al evento de checkpoint alcanzado por el jugador
    }
    private void OnDisable()
    {
        PlayerEvents.OnCheckpointReached -= HandleCheckpointReached; // Desuscribirse del evento de checkpoint alcanzado por el jugador
    }
}
