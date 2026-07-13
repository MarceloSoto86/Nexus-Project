using Unity.Cinemachine;
using UnityEngine;

public class CinemachineZone : MonoBehaviour
{
  [SerializeField] private CinemachineCamera cinemachineCamera; // Referencia a la cámara Cinemachine que se desea controlar

    public int defaultPriority = 10; // Prioridad por defecto de la cámara
    public int activePriority = 20; // Prioridad cuando la cámara está activa
    public int inactivePriority = 10; // Offset para aumentar o disminuir la prioridad


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cinemachineCamera != null) // Verifica si el objeto que entra en la zona es el jugador y si la referencia a la cámara no es nula
        {
            //cinemachineCamera.Priority += 10; // Aumenta la prioridad de la cámara para que se active
            cinemachineCamera.Priority = activePriority; // Establece la prioridad de la cámara a un valor específico para activarla
            //Debug.log($"Cámara {cinemachineCamera.name} activada.");
        }
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.red, 2f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cinemachineCamera != null) // Verifica si el objeto que sale de la zona es el jugador
        {
            //cinemachineCamera.Priority -= 10; // Restaura la prioridad de la cámara para que se desactive
            cinemachineCamera.Priority = inactivePriority; // Establece la prioridad de la cámara a un valor específico para desactivarla
            //Debug.log($"Cámara {cinemachineCamera.name} en espera.");
        }
    }

}
