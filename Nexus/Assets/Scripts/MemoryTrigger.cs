using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    public MemoryManager _memoryManager; // Referencia al MemoryManager para activar el recuerdo
    public MemoryFlashes _memoryData; // ScriptableObject que contiene la información del recuerdo a mostrar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra en el trigger es el jugador
        {
            _memoryManager.TriggerMemory(_memoryData); // Llama al método TriggerMemory del MemoryManager para mostrar el recuerdo
        }
        Destroy(gameObject); // Destruye el trigger después de activarlo para evitar que se active nuevamente
    }
}
