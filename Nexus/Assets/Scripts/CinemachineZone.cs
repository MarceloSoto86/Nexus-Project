using Unity.Cinemachine;
using UnityEngine;

public class CinemachineZone : MonoBehaviour
{
  [SerializeField] private CinemachineCamera cinemachineCamera; // Referencia a la cámara Cinemachine que se desea controlar


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra en la zona es el jugador
        {
            cinemachineCamera.Priority += 10; // Aumenta la prioridad de la cámara para que se active
        }
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.red, 2f);
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que sale de la zona es el jugador
        {
            cinemachineCamera.Priority -= 10; // Restaura la prioridad de la cámara para que se desactive
        }
    }*/

}
