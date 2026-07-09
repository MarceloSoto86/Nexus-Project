using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rayLength = 1.5f;
    //public float health = 100f;
    public float groundCheckDelay = 0.15f; // Distancia para verificar si el jugador está en el suelo
    public float nextGroundCheckTime = 0f; // Tiempo para el próximo chequeo de suelo
    public float maxAirDashWindow = 1.2f; // Tiempo máximo después de despegar del suelo durante el cual el jugador aún puede realizar un dash aéreo (Coyote Dash)
    public float deathYThreshold = -15f; // Altura a la que el jugador muere automáticamente si cae por debajo de ella
    public float nextDashTime { get { return dashSettings.nextDashTime; } set { dashSettings.nextDashTime = value; } }
    public float dashCooldown { get { return dashSettings.dashCooldown; } }

    public int remainingJumps = 2;
    public int maxJumps = 2;
    public float rotationSpeed = 10f; // Velocidad de rotación para orientar al player hacia la dirección del movimiento
    public Vector3 raycastOrigin; // Origen del raycast para verificar si el jugador está en el suelo
    public Vector3 currentDirection;   
    public Transform camTransform; // Referencia al transform de la cámara para orientar la plataforma hacia la cámara
    public GameObject visualRoot; // Referencia al objeto raíz de la parte visual del jugador para rotarlo hacia la cámara sin afectar la física del jugador
    //public Renderer _playerRenderer; // Referencia al componente Renderer del jugador para cambiar su color al recibir daño
    public LayerMask groundLayer; // Capa que representa el suelo para el raycast
    public LayerMask dashZoneLayer; // Asignada en el Inspector a la capa "DashBoosters" o "Cables"
    public bool isDashing = false; // Indica si el jugador está actualmente realizando un dash para evitar que pueda moverse o realizar otras acciones durante el dash
    public bool isDashUnlocked = false; // Indica si el jugador puede realizar un dash (se establece en true cuando el jugador aterriza en el suelo o después de un tiempo de enfriamiento)
    public bool isFlashingDamage = false; // Indica si el jugador está actualmente parpadeando por recibir daño
    public bool jumpPressed;
    public bool isStunned = false; // Indica si el jugador está actualmente aturdido por recibir daño
    public Rigidbody rb;

    public static PlayerController player;
    public PlayerDash dashSettings; // Arrastrá el script PlayerDash aquí en el Inspector
    public UnityEngine.UI.Image dashHUDIcon;

    public GhostEffect ghostEffect; // Referencia al componente GhostEffect para generar el efecto fantasma al ejecutar el dash
    public PlayerBaseState currentState;
    public PlayerBaseState idleState = new PlayerIdleState();
    public PlayerBaseState walkingState = new PlayerWalkingState();
    public PlayerBaseState runningState = new PlayerRunningState();
    public PlayerBaseState jumpingState = new PlayerJumpingState();
    public PlayerBaseState fallingToLandingState = new PlayerFallingToLandingState();
    public PlayerBaseState dashingState = new PlayerDashingState();
    public PlayerBaseState isFlashingDamageState = new PlayerFlashingDamageState();
    public PlayerBaseState dyingFromInsanityState = new PlayerDeathFromInsanityState();
    public PlayerBaseState dyingFromDamageState = new PlayerDeathFromDmgState();
    public PlayerBaseState panickedFallingState = new PlayerPanickedFallingState();
    //public RotatePlatform currentPlatform;
    public Animator animator; // Referencia al componente Animator del jugador para controlar las animaciones del jugador
 
    private Vector3 previousPos;
    private Vector3 checkpointPos;
    private float _lastimeGrounded; // Variable para almacenar el último tiempo que el jugador estuvo en el suelo, para dar un pequeño margen de tiempo para permitir saltar después de despegar del suelo (coyote time)


    private void Start()
    {
        camTransform = Camera.main.transform; // Obtiene la referencia al transform de la cámara principal
        player = this; // Asigna la instancia actual del jugador a la variable estática para que pueda ser accedida desde otros scripts
        rb = GetComponent<Rigidbody>();
        previousPos = transform.position;

        checkpointPos = transform.position; // Establece la posición inicial del jugador como el primer checkpoint

        currentState = idleState; // Establece el estado inicial del jugador como idle
        currentState.EnterState(this); // Llama al método EnterState del estado inicial para realizar cualquier configuración o inicialización necesaria para ese estado
    }
    void Update()
    {
        currentState.UpdateState(this); // Llama al método UpdateState del estado actual para manejar la lógica de ese estado
        CalculateDirection();
        CheckGroundedStatus();
        // DEBUG: Esto te dirá en la consola qué está pasando con el Animator
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            // Debug.Log("Animación actual en ejecución: " + info.shortNameHash);

        }
        // DEBUG DE ANIMACIÓN
        if (Input.GetKeyDown(KeyCode.T)) // Presioná la T mientras jugás
        {
            if (animator != null)
            {
                Debug.Log("Intentando forzar Idle manualmente...");
                animator.Play("a_Idle");
            }
            else
            {
                Debug.LogError("¡El componente Animator no está asignado!");
            }
        }
    }

    public bool IsGrounded(float checkDistance)
    {
        raycastOrigin = transform.position + (Vector3.up * 0.5f); // Ajusta el origen del raycast ligeramente por encima del centro del jugador para evitar colisiones con suelo
        return Physics.Raycast(raycastOrigin, Vector3.down, checkDistance, groundLayer, QueryTriggerInteraction.Ignore);// Realiza un raycast hacia abajo
        //RotatePlatform platform = hit.collider.GetComponent<RotatePlatform>();
    }

    /* public bool IsGrounded()
     {
         raycastOrigin = transform.position + (Vector3.up * 0.5f); // Ajusta el origen del raycast ligeramente por encima del centro del jugador para evitar colisiones con suelo
         return Physics.Raycast(raycastOrigin, Vector3.down, rayLength, groundLayer, QueryTriggerInteraction.Ignore);// Realiza un raycast hacia abajo
     }*/
    public void CheckGroundedStatus() // Método para verificar si el jugador está en el suelo utilizando un raycast
    { 
        bool isGrounded = IsGrounded(rayLength); // Verifica si el jugador está en el suelo utilizando el método IsGrounded
        _lastimeGrounded = isGrounded ? Time.time : _lastimeGrounded; // Actualiza el último tiempo que el jugador estuvo en el suelo si está actualmente en el suelo
        Debug.DrawRay(raycastOrigin, Vector3.down * rayLength, Color.green);
        // Solo reseteamos saltos, no escuchamos el espacio aquí.
        if (isGrounded && Time.time > nextGroundCheckTime)// Si el jugador está en el suelo y se ha presionado la barra espaciadora para saltar, restablece los saltos disponibles
        {
            if (rb.linearVelocity.y <= 0.1f)
            {
                remainingJumps = maxJumps; // Restablece los saltos disponibles al aterrizar
                jumpPressed = false; // Reinicia el estado de salto después de realizar un salto

                // --- RECARGA DEL DASH TERRESTRE ---
                dashSettings.canDashInAir = true;
            }
        }  
        
        else
        {
            if (remainingJumps == maxJumps)
            {
                remainingJumps = maxJumps - 1;
            }
        }
    }
    public void CalculateDirection()
    {
        // Calcula la dirección del movimiento horizontal comparando la posición actual con la posición anterior
        Vector3 movement = transform.position - previousPos;
        movement  = new Vector3(movement.x, 0f, movement.z); // Ignora el movimiento vertical para calcular la dirección horizontal
        if (movement != Vector3.zero)
        {
            currentDirection = movement.normalized;
        }
        // Actualiza la posición anterior para la próxima comparación
        previousPos = transform.position;
    }  
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        checkpointPos = checkpointPosition; // Actualiza la posición del checkpoint
    }
    public void Respawn()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Restablece la velocidad del jugador antes de teletransportarlo al checkpoint
            rb.angularVelocity = Vector3.zero; // Restablece la velocidad angular del jugador antes de teletransportarlo al checkpoint
        }
        transform.position = checkpointPos; // Teletransporta al jugador a la posición del checkpoint

        Physics.SyncTransforms(); // Sincroniza la posición del Rigidbody con la posición del Transform para evitar problemas de colisiones o física al reaparecer

        //rb.MovePosition(checkpointPos); // Asegura que el Rigidbody del jugador se mueva a la posición del checkpoint para evitar problemas de colisiones o física al reaparecer
        remainingJumps = maxJumps; // Restablece los saltos disponibles al reaparecer

        SwitchState(idleState); // Cambia el estado del jugador a idle al reaparecer
    }  
    public IEnumerator DamageFlash()
    {
        if (visualRoot != null)
        {
            isFlashingDamage = true; // Indica que el jugador está actualmente parpadeando por recibir daño
            Renderer[] renderers = visualRoot.GetComponentsInChildren<Renderer>(); // Obtiene todos los componentes Renderer en el objeto visual del jugador        
        foreach (var r in renderers) // Usamos MaterialPropertyBlock para un mejor rendimiento en Unity 6
            {
                // En URP el nombre técnico es _BaseColor
                r.material.SetColor("_BaseColor", Color.red);
            }
            yield return new WaitForSeconds(0.2f);
            foreach (var r in renderers)
            {
                // Aquí puedes intentar resetearlo o guardar el color original antes
                r.material.SetColor("_BaseColor", Color.white);
            }
            isFlashingDamage = false;
        }
    }
    public void ApplyKnockback(Vector3 knockbackDirection, float knockbackForce)
    {
        StartCoroutine(StunPlayerRoutine(knockbackDirection, knockbackForce)); // Inicia la rutina de aturdimiento y aplicación de knockback  
    }

    public bool IsPathSafe()
    {
        // 1. Realiza un raycast en la dirección del movimiento para verificar si hay obstáculos en el camino
        if (IsGrounded(rayLength)) return true; // Si el jugador está en el suelo, el camino es seguro

        // 2. Si el jugador no está en el suelo, realiza un raycast en la dirección del movimiento para verificar si hay obstáculos en el camino
        float maxSafeFallDistance = 15f; // Distancia máxima que el jugador puede caer sin morir

        // Si el tiempo transcurrido desde que dejó el suelo es menor al margen de gracia...
        if (Time.time - _lastimeGrounded <= maxAirDashWindow)
        {
            return true; // Da luz verde inmediata por habilidad de salida (Coyote Dash)
        }
        // Calculamos un vector diagonal hacia adelante y abajo
        Vector3 diagonalDir = (Vector3.down + currentDirection).normalized;

        bool hitGround = Physics.Raycast(transform.position, Vector3.down, maxSafeFallDistance, groundLayer, QueryTriggerInteraction.Ignore); // Realiza un raycast hacia abajo para verificar si hay suelo debajo del jugador dentro de la distancia segura de caída
        bool hitDashZone = Physics.Raycast(transform.position, diagonalDir, maxSafeFallDistance, dashZoneLayer, QueryTriggerInteraction.Ignore); // Realiza un raycast hacia abajo para verificar si hay una zona de dash debajo del jugador dentro de la distancia segura de caída

        if(hitGround || hitDashZone) // Si el raycast golpea algo, verifica si es un terreno seguro (puedes usar capas o tags para identificar terrenos seguros)
        {
            return true; // Si golpea algo, el camino es seguro
        }
        else
        {
            Debug.Log("¡Dash bloqueado! No hay terreno seguro debajo del jugador.");
            return false; // Si no golpea nada, el camino no es seguro
        }

        /*bool hitSomething = IsGrounded(maxSafeFallDistance); // Verifica si hay algo debajo del jugador dentro de la distancia segura de caída

        // 3. Si el raycast golpea algo, verifica si es un terreno seguro (puedes usar capas o tags para identificar terrenos seguros)
        if (!hitSomething)
        {
           Debug.Log("¡Dash bloqueado! No hay terreno seguro debajo del jugador.");
            return false; // Si no golpea nada, el camino no es seguro
        }
        return true; // Si golpea algo, el camino es seguro*/
    }
    IEnumerator StunPlayerRoutine(Vector3 knockbackDirection, float knockbackForce)
    {
        isStunned = true; // Indica que el jugador está actualmente aturdido por recibir daño
        rb.linearVelocity = Vector3.zero; // Restablece la velocidad del jugador antes de aplicar el knockback para evitar que se mantenga el impulso actual
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f); // Duración del aturdimiento (puede ser ajustada según tus necesidades)
        isStunned = false; // Indica que el jugador ha terminado de estar aturdido y puede moverse nuevamente
    }
    public void SwitchState(PlayerBaseState newState)
    {
        if(currentState == newState) return; // Si el nuevo estado es el mismo que el estado actual, no realiza ningún cambio para evitar reiniciar la lógica del estado innecesariamente
        currentState = newState; // Cambia el estado actual del jugador al nuevo estado
        currentState.EnterState(this); // Llama al método EnterState del nuevo estado para realizar cualquier configuración o inicialización necesaria para ese estado
    }
}
