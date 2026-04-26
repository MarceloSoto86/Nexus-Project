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
    public GhostEffect _ghostEffect; // Referencia al componente GhostEffect para generar el efecto fantasma al ejecutar el dash



    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene la referencia al Rigidbody del jugador para poder manipular su posición y movimiento durante el dash
        _ghostEffect = GetComponent<GhostEffect>(); // Obtiene la referencia al componente GhostEffect para poder activar el efecto de estela fantasma durante el dash, lo que puede mejorar la sensación de velocidad y dinamismo durante el dash
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
        playerController.isDashing = true; // Establece la variable isDashing en el PlayerController a true para indicar que el jugador está actualmente realizando un dash, lo que puede ser utilizado para controlar otras mecánicas o animaciones relacionadas con el dash
        rb.useGravity = false; // Desactiva la gravedad durante el dash para que el jugador no caiga mientras se teletransporta
        rb.linearVelocity = Vector3.zero; // Detiene el movimiento actual del jugador antes de teletransportarlo para evitar que el impulso anterior afecte el teletransporte   
        _ghostEffect.StartTrail(); // Inicia el efecto fantasma para crear una estela visual durante el dash, lo que puede mejorar la sensación de velocidad y dinamismo durante el dash

        Vector3 dashDir = currentDirection; // Calcula la dirección del dash basada en la dirección actual del jugador
        if(dashDir.sqrMagnitude < 0.001f) // Si la dirección del dash es muy pequeña (casi cero) se asigna una dirección predeterminada para evitar problemas de teletransporte sin dirección
        {
            dashDir = transform.forward; // Asigna la dirección del dash hacia adelante como valor predeterminado si la dirección actual es demasiado pequeña para ser válida
        }
        
        dashDir.y = 0; // Asegura que el dash se realice solo en el plano horizontal al eliminar cualquier componente vertical de la dirección del dash
        dashDir.Normalize(); // Normaliza la dirección del dash para asegurarse de que tenga una magnitud de 1, lo que permite que el dash se realice a una distancia constante independientemente de la magnitud de la dirección original
        


        bool raycastHitWall = Physics.Raycast(transform.position + Vector3.up * 0.5f, currentDirection, out RaycastHit hitInfo, dashDistance, wallLayerMask); // Realiza un raycast en la dirección actual para verificar si hay una pared delante del jugador antes de permitir el teletransporte
        Vector3 newDesiredPos = transform.position + currentDirection * dashDistance; // Teletransporta al jugador en la dirección actual multiplicada por una distancia de teletransporte (ajusta el valor según sea necesario)
        if (raycastHitWall)
        {
            newDesiredPos = hitInfo.point - wallDistanceOffset * currentDirection;
            Debug.Log("Raycast tocando: " + hitInfo.collider.name); // Imprime el nombre del objeto que el raycast ha tocado para verificar que se esté detectando correctamente la pared
        }
         //rb.MovePosition(newDesiredPos); // Utiliza MovePosition para teletransportar al jugador a la nueva posición calculada
        // Dentro de DashRoutine, en lugar de MovePosition:
        float dashSpeed = dashDistance / dashDuration;
        rb.linearVelocity = dashDir * dashSpeed;

        Debug.Log("Current Direction: " + currentDirection); // Imprime la dirección actual en la consola para verificar que se esté calculando correctamente
        yield return new WaitForSeconds(dashDuration);
        yield return new WaitForFixedUpdate(); // <--- Esto asegura que el frenazo coincida con el motor de física

        rb.linearVelocity = Vector3.zero; // Detiene cualquier movimiento residual después del dash para evitar que el jugador siga moviéndose después de teletransportarse
        rb.angularVelocity = Vector3.zero; // Detiene cualquier rotación residual después del dash para evitar que el jugador siga rotando después de teletransportarse
        
        rb.useGravity = true; // Reactiva la gravedad después de que el dash haya terminado para que el jugador pueda moverse normalmente después de teletransportarse
        rb.MovePosition(transform.position); // Asegura que el jugador esté en la posición correcta después del dash para evitar problemas de colisión o desincronización de la posición después de teletransportarse
        _ghostEffect.StopTrail(); // Detiene el efecto fantasma después de que el dash haya terminado para limpiar la estela visual y evitar que el efecto continúe indefinidamente, lo que puede mejorar la sensación de control y fluidez en el movimiento después del dash
        playerController.isDashing = false; // Establece la variable isDashing en el PlayerController a false para indicar que el jugador ha terminado de realizar el dash
    }
}
