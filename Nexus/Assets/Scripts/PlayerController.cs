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
    public int remainingJumps = 2;
    public int maxJumps = 2;
    public float rotationSpeed = 10f; // Velocidad de rotación para orientar al player hacia la dirección del movimiento
    public Vector3 raycastOrigin; // Origen del raycast para verificar si el jugador está en el suelo
    public Vector3 currentDirection;   
    public Transform camTransform; // Referencia al transform de la cámara para orientar la plataforma hacia la cámara
    public GameObject visualRoot; // Referencia al objeto raíz de la parte visual del jugador para rotarlo hacia la cámara sin afectar la física del jugador
    //public Renderer _playerRenderer; // Referencia al componente Renderer del jugador para cambiar su color al recibir daño
    public LayerMask groundLayer; // Capa que representa el suelo para el raycast
    public bool isDashing = false; // Indica si el jugador está actualmente realizando un dash para evitar que pueda moverse o realizar otras acciones durante el dash
    public bool isFlashingDamage = false; // Indica si el jugador está actualmente parpadeando por recibir daño
    public bool jumpPressed;
    public bool isStunned = false; // Indica si el jugador está actualmente aturdido por recibir daño
    public Rigidbody rb;

    public static PlayerController player;
    public PlayerDash dashSettings; // Arrastrá el script PlayerDash aquí en el Inspector
    public float nextDashTime { get { return dashSettings.nextDashTime; } set { dashSettings.nextDashTime = value; } }
    public float dashCooldown { get { return dashSettings.dashCooldown; } }
    public GhostEffect ghostEffect; // Referencia al componente GhostEffect para generar el efecto fantasma al ejecutar el dash
    public PlayerBaseState currentState;
    public PlayerBaseState idleState = new PlayerIdleState();
    public PlayerBaseState walkingState = new PlayerWalkingState();
    public PlayerBaseState runningState = new PlayerRunningState();
    public PlayerBaseState jumpingState = new PlayerJumpingState();
    public PlayerBaseState fallingToLandingState = new PlayerFallingToLandingState();
    public PlayerBaseState dashingState = new PlayerDashingState();
    public PlayerBaseState isFlashingDamageState = new PlayerFlashingDamageState();
    public Animator animator; // Referencia al componente Animator del jugador para controlar las animaciones del jugador

   
    private Vector3 previousPos;
    private Vector3 checkpointPos;

    
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
    public bool IsGrounded()
    {
        raycastOrigin = transform.position + (Vector3.up * 0.5f); // Ajusta el origen del raycast ligeramente por encima del centro del jugador para evitar colisiones con suelo
        return Physics.Raycast(raycastOrigin, Vector3.down, rayLength, groundLayer, QueryTriggerInteraction.Ignore);// Realiza un raycast hacia abajo
    }
    public void CheckGroundedStatus() // Método para verificar si el jugador está en el suelo utilizando un raycast
    { 
        bool isGrounded = IsGrounded(); // Verifica si el jugador está en el suelo utilizando el método IsGrounded
        Debug.DrawRay(raycastOrigin, Vector3.down * rayLength, Color.green);
        // Solo reseteamos saltos, no escuchamos el espacio aquí.
        if (isGrounded && Time.time > nextGroundCheckTime)// Si el jugador está en el suelo y se ha presionado la barra espaciadora para saltar, restablece los saltos disponibles
        {
            if (rb.linearVelocity.y <= 0.1f)
            {
                remainingJumps = maxJumps; // Restablece los saltos disponibles al aterrizar
                jumpPressed = false; // Reinicia el estado de salto después de realizar un salto
            }
        }  
        if (isGrounded)
        {
            //Debug.Log("Raycast tocando: " + groundLayer.value);
        }
        else
        {
            //Debug.Log("Raycast al aire");
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
        transform.position = checkpointPos; // Teletransporta al jugador a la posición del checkpoint
        rb.linearVelocity = Vector3.zero; // Restablece la velocidad del jugador para evitar que se mantenga el impulso después de reaparecer
        rb.angularVelocity = Vector3.zero; // Restablece la velocidad angular del jugador para evitar que gire después de reaparecer
        rb.MovePosition(checkpointPos); // Asegura que el Rigidbody del jugador se mueva a la posición del checkpoint para evitar problemas de colisiones o física al reaparecer
        remainingJumps = maxJumps; // Restablece los saltos disponibles al reaparecer
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
