using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rayLength = 1.5f;
    public float health = 100f;
    public int remainingJumps = 2;
    public int maxJumps = 2;
    public float rotationSpeed = 10f; // Velocidad de rotación para orientar al player hacia la dirección del movimiento
    public Vector3 raycastOrigin; // Origen del raycast para verificar si el jugador está en el suelo
    public Vector3 currentDirection;
    public bool jumpPressed;
    public Transform camTransform; // Referencia al transform de la cámara para orientar la plataforma hacia la cámara
    public Renderer _playerRenderer; // Referencia al componente Renderer del jugador para cambiar su color al recibir daño
    public LayerMask groundLayer; // Capa que representa el suelo para el raycast

    public static PlayerController player;
    private float groundCheckDelay = 0.15f; // Distancia para verificar si el jugador está en el suelo
    private float nextGroundCheckTime = 0f; // Tiempo para el próximo chequeo de suelo
    private Rigidbody rb;
    private Vector3 previousPos;
    private Vector3 checkpointPos;
    private bool isFlashingDamage = false; // Indica si el jugador está actualmente parpadeando por recibir daño
    private bool isStunned = false; // Indica si el jugador está actualmente aturdido por recibir daño

    private void Start()
    {
        camTransform = Camera.main.transform; // Obtiene la referencia al transform de la cámara principal
        player = this; // Asigna la instancia actual del jugador a la variable estática para que pueda ser accedida desde otros scripts
        rb = GetComponent<Rigidbody>();
        previousPos = transform.position;
        checkpointPos = transform.position; // Establece la posición inicial del jugador como el primer checkpoint
    }
    void Update()
    {
        CalculateDirection();
        //SetCheckpoint(checkpointPos); // Asegura que la posición del checkpoint se actualice constantemente, aunque en este caso no cambia a menos que se llame explícitamente a SetCheckpoint con una nueva posición

        raycastOrigin = transform.position + (Vector3.up * 0.1f); // Ajusta el origen del raycast ligeramente por encima del centro del jugador para evitar colisiones con el suelo
        bool isGrounded = Physics.Raycast(raycastOrigin, Vector3.down, rayLength, groundLayer); // Realiza un raycast hacia abajo para verificar si el jugador está en el suelo
        Debug.DrawRay(raycastOrigin, Vector3.down * rayLength, Color.green);
        // Si el jugador está en el suelo y se ha presionado la barra espaciadora para saltar, restablece los saltos disponibles
        if (isGrounded && rb.linearVelocity.y <=0.1f && Time.time > nextGroundCheckTime)
        {
            jumpPressed = false; // Reinicia el estado de salto después de realizar un salto
            remainingJumps = maxJumps; // Restablece los saltos disponibles al aterrizar
        }
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0) // Si el jugador está en el suelo y presiona la barra espaciadora, realiza un salto siempre que tenga saltos disponibles
        {
            if (remainingJumps > 0)
            {
                remainingJumps--;
                Jump();
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
    void FixedUpdate()
    {
        // Obtiene la entrada del jugador para el movimiento horizontal y vertical
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcula el movimiento en función de la entrada del jugador y la orientación de la cámara
        Vector3 forwardCam = camTransform.forward.normalized; // Obtiene la dirección hacia adelante de la cámara
        forwardCam.y = 0; // Elimina la componente vertical para que la plataforma solo se oriente en el plano horizontal
        forwardCam.Normalize(); // Normaliza la dirección para mantener una velocidad constante
        Vector3 rightCam = camTransform.right.normalized; // Obtiene la dirección hacia la derecha de la cámara
        rightCam.y = 0; // Elimina la componente vertical para que la plataforma solo se oriente en el plano horizontal
        rightCam.Normalize(); // Normaliza la dirección para mantener una velocidad constante
        Vector3 desiredMove = forwardCam * verticalInput + rightCam * horizontalInput; // Calcula el movimiento deseado en función de la orientación de la cámara (en este caso, no se mueve)
        if(!isStunned) // Solo permite el movimiento si el jugador no está aturdido por recibir daño
        { 
            if (desiredMove.magnitude > 1f)
            {
                desiredMove.Normalize(); // Normaliza el movimiento deseado para mantener una velocidad constante incluso cuando se mueve en diagonal
            }

            // Aplica el movimiento al Rigidbody del jugador multiplicando por la velocidad de movimiento para controlar la velocidad del jugador
            rb.linearVelocity = new Vector3(desiredMove.x * moveSpeed, rb.linearVelocity.y, desiredMove.z * moveSpeed);

            if(desiredMove != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredMove); // Calcula la rotación objetivo para orientar al player hacia la dirección del movimiento
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); // Suaviza la rotación del player hacia la rotación objetivo utilizando Slerp
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
    // Aplica una fuerza hacia arriba para realizar un salto y establece el estado de salto para evitar que el jugador pueda saltar nuevamente hasta que aterrice
    public void Jump()
    {
        // Antes de aplicar la fuerza de salto, restablece la velocidad vertical del jugador a cero para evitar que el salto se vea afectado por la velocidad actual
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;
        // Aplica una fuerza hacia arriba para realizar el salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpPressed = true; // Establece el estado de salto para evitar que el jugador pueda saltar nuevamente hasta que aterrice
        nextGroundCheckTime = Time.time + groundCheckDelay; // Establece el tiempo para el próximo chequeo de suelo después de realizar un salto
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
        remainingJumps = maxJumps; // Restablece los saltos disponibles al reaparecer
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // Resta el daño recibido a la salud del jugador
        if (health <= 0f)
        {
            health = 0f; // Asegura que la salud no sea negativa
            Respawn(); // Si la salud llega a cero o menos, el jugador reaparece en el checkpoint
        }
        else
        {
            if (!isFlashingDamage)
            {
                StartCoroutine(DamageFlash()); // Inicia la rutina de parpadeo de daño si el jugador recibe daño pero no muere
            }
        }
    }
    IEnumerator DamageFlash()
    {
        if (_playerRenderer != null)
        {
            isFlashingDamage = true; // Indica que el jugador está actualmente parpadeando por recibir daño
            Color originalColor = _playerRenderer.material.color; // Guarda el color original del jugador
            _playerRenderer.material.color = Color.red; // Cambia el color del jugador a rojo para indicar que ha recibido daño
            yield return new WaitForSeconds(0.2f); // Espera un breve momento antes de restablecer el color
            _playerRenderer.material.color = originalColor; // Restablece el color original del jugador después de recibir daño
            isFlashingDamage = false; // Indica que el jugador ha terminado de parpadear por recibir daño
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
}
