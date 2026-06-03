using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    //public MemoryManager _memoryManager; // Referencia al MemoryManager para activar el recuerdo
    public MemoryFlashes _memoryData; // ScriptableObject que contiene la información del recuerdo a mostrar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra en el trigger es el jugador
        {
            if (MemoryManager.Instance != null && _memoryData != null)
            {
                //Buscamos el UIManager y mostramos el mensaje de memoria utilizando el texto del ScriptableObject. El mensaje se mostrará durante 4,5 segundos.
                MemoryManager.Instance.TriggerMemory(_memoryData); // Muestra el mensaje de memoria en el UIManager
            }
        }
        Destroy(gameObject); // Destruye el trigger después de activarlo para evitar que se active nuevamente
    }
}
