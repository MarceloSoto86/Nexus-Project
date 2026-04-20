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

    private Rigidbody rb; // Referencia al Rigidbody del jugador



    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene la referencia al Rigidbody del jugador para poder manipular su posición y movimiento durante el dash
    }

    // Update is called once per frame
    public void Update()
    {
        if (playerController != null)
        {
            currentDirection = playerController.currentDirection; // Actualiza la dirección actual del jugador en cada frame para asegurarse de que el dash se realice en la dirección correcta según la entrada del jugador o la dirección de movimiento actual
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextDashTime) // Si el jugador presiona la tecla E para realizar el dash y el tiempo actual es mayor o igual al tiempo para el próximo dash disponible, lo que asegura que el jugador solo pueda realizar un dash si ha pasado el tiempo de enfriamiento desde el último dash
        {
            StartCoroutine(DashRoutine()); // Inicia la rutina de dash para realizar el teletransporte y manejar el enfriamiento entre dashes
            Debug.Log("Dash");

            nextDashTime = Time.time + dashCooldown; // Actualiza el tiempo para el próximo dash disponible sumando el tiempo actual con el tiempo de enfriamiento para evitar que el jugador pueda realizar dashes consecutivos sin pausa
        }
    }

    IEnumerator DashRoutine()
    {
        rb.useGravity = false; // Desactiva la gravedad durante el dash para que el jugador no caiga mientras se teletransporta, lo que puede mejorar la sensación de movimiento y control durante el dash
        rb.linearVelocity = Vector3.zero; // Detiene el movimiento actual del jugador antes de teletransportarlo para evitar que el impulso anterior afecte el teletransporte   

        bool raycastHitWall = Physics.Raycast(transform.position + Vector3.up * 0.5f, currentDirection, out RaycastHit hitInfo, dashDistance, wallLayerMask); // Realiza un raycast en la dirección actual para verificar si hay una pared delante del jugador antes de permitir el teletransporte
        Vector3 newDesiredPos = transform.position + currentDirection * dashDistance; // Teletransporta al jugador en la dirección actual multiplicada por una distancia de teletransporte (ajusta el valor según sea necesario)
        if (raycastHitWall)
        {
            newDesiredPos = hitInfo.point - wallDistanceOffset * currentDirection;
            Debug.Log("Raycast tocando: " + hitInfo.collider.name); // Imprime el nombre del objeto que el raycast ha tocado para verificar que se esté detectando correctamente la pared
        }
        rb.MovePosition(newDesiredPos); // Utiliza MovePosition para teletransportar al jugador a la nueva posición calculada

        Debug.Log("Current Direction: " + currentDirection); // Imprime la dirección actual en la consola para verificar que se esté calculando correctamente

        yield return new WaitForSeconds(dashDuration); // Espera la duración del dash antes de permitir que el jugador se mueva nuevamente, lo que puede mejorar la sensación de control y fluidez en el movimiento después del dash
        rb.linearVelocity = Vector3.zero; // Detiene cualquier movimiento residual después del dash para evitar que el jugador siga moviéndose después de teletransportarse, lo que puede mejorar la sensación de control y fluidez en el movimiento después del dash
        rb.useGravity = true; // Reactiva la gravedad después de que el dash haya terminado para que el jugador pueda moverse normalmente después de teletransportarse, lo que puede mejorar la sensación de control y fluidez en el movimiento después del dash
    }
}
