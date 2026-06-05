using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f; // Distancia que el jugador se teletransportará al realizar el dash
    public float wallDistanceOffset = 0.5f; // Distancia adicional para mantener al jugador alejado de la pared después del teletransporte
    public float dashCooldown = 1f; // Tiempo de enfriamiento entre cada dash para evitar que el jugador pueda realizar dashes consecutivos sin pausa
    public float dashDuration = 0.2f; // Duración del dash, que puede afectar la velocidad o el tiempo que el jugador permanece en la posición teletransportada antes de poder moverse nuevamente
    public float nextDashTime = 0f; // Tiempo para el próximo dash disponible, que se actualizará cada vez que el jugador realice un dash para implementar el enfriamiento entre dashes
    public Vector3 currentDirection; // Dirección actual del jugador, que se actualizará en función de la entrada del jugador o la dirección de movimiento
    public PlayerController playerController; // Referencia al script PlayerController para acceder a la dirección actual del jugador y otras variables relacionadas con el movimiento
    public LayerMask wallLayerMask; // Capa que representa las paredes en el juego para que el raycast pueda detectar correctamente las colisiones con las paredes durante el dash
    public bool useGravityDuringDash = false; // Opción para determinar si el jugador debe ser afectado por la gravedad durante el dash, lo que puede afectar la sensación de movimiento y control durante el dash
    public bool canDashInAir = true; // Opción para permitir que el jugador pueda realizar dashes mientras está en el aire, lo que puede agregar más dinamismo al movimiento del jugador y permitir estrategias de movilidad más variadas

    private Rigidbody rb; // Referencia al Rigidbody del jugador
   
}
