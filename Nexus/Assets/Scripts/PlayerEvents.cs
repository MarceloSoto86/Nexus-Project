using UnityEngine;
using System;

public class PlayerEvents : MonoBehaviour
{
    // --- EVENTOS DE CHECKPOINT EXISTENTES ---
    public static event Action<float> OnCheckpointReached;
    private float _maxHeightReached;

    // Transmite: (Cordura Actual, Slots Desbloqueados)
    public static event Action<float, int> OnSanityChanged;
    // Alerta de expansión para cuando la trama abre una nueva celda
    public static event Action OnMemoryStoreUnlocked;

    private void Start()
    {
        _maxHeightReached = transform.position.y; // Inicializa la altura máxima alcanzada con la posición inicial del jugador
    }
    //Este método se llama cuando el jugador alcanza un nuevo punto de control para actualizar la posición del último punto de control alcanzado
    public void TriggerCheckpoint(float checkpointHeight)
    {
        OnCheckpointReached?.Invoke(checkpointHeight); // Invoca el evento de punto de control alcanzado por el jugador
        Debug.Log("Evento lanzado: " + checkpointHeight);
    }
    // --- MÉTODOS DISPARADORES PARA EL STATUS ---
    public void RaiseSanityChanged(float currentSanity, int unlockedSlots)
    {
        OnSanityChanged?.Invoke(currentSanity, unlockedSlots);
    }

    public void RaiseMemoryStoreUnlocked()
    {
        OnMemoryStoreUnlocked?.Invoke();
    }
}
