using UnityEngine;


public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController player); // This method will be called when the player enters this state, allowing you to initialize any necessary variables or perform setup actions specific to this state.
    public abstract void UpdateState(PlayerController player); // This method will be called every frame while the player is in this state, allowing you to handle input and update the player's behavior accordingly.

    public abstract void CheckSwitchState(PlayerController player); // This method will be responsible for checking if the player should switch to another state based on input or conditions.

    protected void HandleGlobalInputs(PlayerController player)
    {
        // Detección de salto
        if (Input.GetKeyDown(KeyCode.Space) && player.remainingJumps > 0)
        {
            player.SwitchState(player.jumpingState); // El JumpingState se encargará del resto
        }

        // Dash con E y Cooldown
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= player.dashSettings.nextDashTime)
        {
            player.dashSettings.nextDashTime = Time.time + player.dashSettings.dashCooldown;
            player.SwitchState(player.dashingState);
        }
    }
    protected bool Move(PlayerController player)
    {
        // Obtiene la entrada del jugador para el movimiento horizontal y vertical
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forwardCam = player.camTransform.forward.normalized; // Obtiene la dirección hacia adelante de la cámara
        forwardCam.y = 0; // Elimina la componente vertical para que la plataforma solo se oriente en el plano horizontal
        forwardCam.Normalize(); // Normaliza la dirección para mantener una velocidad constante
        Vector3 rightCam = player.camTransform.right.normalized; // Obtiene la dirección hacia la derecha de la cámara
        rightCam.y = 0; // Elimina la componente vertical para que la plataforma solo se oriente en el plano horizontal
        rightCam.Normalize(); // Normaliza la dirección para mantener una velocidad constante

        Vector3 desiredMove = forwardCam * verticalInput + rightCam * horizontalInput; // Calcula el movimiento deseado en función de la orientación de la cámara (en este caso, no se mueve)

        if (!player.isStunned && !player.isDashing) // Solo permite el movimiento si el jugador no está aturdido por recibir daño
        {
            if (desiredMove.magnitude > 1f) desiredMove.Normalize(); // Normaliza el movimiento deseado para mantener una velocidad constante incluso cuando se mueve en diagonal

            // Aplica el movimiento al Rigidbody del jugador multiplicando por la velocidad de movimiento para controlar la velocidad del jugador
            player.rb.linearVelocity = new Vector3(desiredMove.x * player.moveSpeed, player.rb.linearVelocity.y, desiredMove.z * player.moveSpeed);

            // Rotación y retorno de "isMoving"
            if (desiredMove != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredMove); // Calcula la rotación objetivo para orientar al player hacia la dirección del movimiento

                if (player.visualRoot != null) // Si el visualRoot no es nulo, rota el visualRoot en lugar del player para evitar problemas de colisiones
                { 
                    player.visualRoot.transform.rotation = Quaternion.Slerp(player.visualRoot.transform.rotation, targetRotation, player.rotationSpeed * Time.deltaTime); //Suaviza la rotación del visualRoot
                };
            
                return true; // Indica que el movimiento se ha manejado y no es necesario cambiar de estado
            }
        }
        return false; // Indica que el movimiento se ha manejado y no es necesario cambiar de estado
    }

    protected void HandleFallingAndLanding(PlayerController player)
    {
       
        
        if (player.rb.linearVelocity.y < -0.1f) // Si el jugador está cayendo, cambia al estado de caída
        {
            if (player.transform.position.y < player.deathYThreshold && player.currentState != player.panickedFallingState)
            {
                player.SwitchState(player.panickedFallingState);
                return;
            }
            // Lanzamos un rayo exclusivo para aterrizar, mucho más corto que el normal
            Vector3 origin = player.transform.position + (Vector3.up * 0.1f);
            float strictLandingDistance = 0.2f; // Distancia más corta para detectar el suelo al aterrizar

            bool nearGround = Physics.Raycast(origin, Vector3.down, strictLandingDistance, player.groundLayer, QueryTriggerInteraction.Ignore);

            if (nearGround) // Si el jugador está cayendo y aterriza en el suelo, cambia al estado de caída a aterrizaje
            {
                // FRENADO VERTICAL: Evita que la inercia lo hunda en el suelo
                Vector3 vel = player.rb.linearVelocity;
                vel.y = 0;
                player.rb.linearVelocity = vel;

                player.SwitchState(player.fallingToLandingState);
            }
            
        } 
    }
}
