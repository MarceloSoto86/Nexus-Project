using UnityEngine;
using System;

public class PlayerEvents : MonoBehaviour
{
    //public static event Action<float> OnPlayerHeightReached;
    public static event Action<float> OnCheckpointReached;
    private float _maxHeightReached;
    //private Vector3 lastCheckPointReached;

    private void Start()
    {
        _maxHeightReached = transform.position.y; // Inicializa la altura máxima alcanzada con la posición inicial del jugador
    }

   /* private void Update()
    {
        // Si el jugador ha alcanzado un nuevo punto de control, actualizar la posición del último punto de control alcanzado
        if (transform.position.y > _maxHeightReached) // Verifica si la altura actual del jugador es mayor que la altura máxima alcanzada previamente
        {
            _maxHeightReached = transform.position.y; // Cada vez que el jugador alcanza una nueva altura, se actualiza la variable _maxHeightReached con la nueva altura alcanzada por el jugador
            OnPlayerHeightReached?.Invoke(_maxHeightReached); // Invoca el evento de altura alcanzada por el jugador

            Debug.Log("Evento lanzado: " + _maxHeightReached);
        }
    }*/

    //Este método se llama cuando el jugador alcanza un nuevo punto de control para actualizar la posición del último punto de control alcanzado
    public void TriggerCheckpoint(float checkpointHeight)
    {
        OnCheckpointReached?.Invoke(checkpointHeight); // Invoca el evento de punto de control alcanzado por el jugador
        Debug.Log("Evento lanzado: " + checkpointHeight);
    }
}
