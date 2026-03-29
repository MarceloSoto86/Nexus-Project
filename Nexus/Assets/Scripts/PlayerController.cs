using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rayLength = 1.1f;
    public int remainingJumps = 2;
    public int maxJumps = 2;
    private Rigidbody rb;
    private Vector3 previousPos;
    public Vector3 currentDirection;
    public static PlayerController player;
    public bool jumpPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPos = transform.position;
    }

    void Update()
    {
        CalculateDirection();
        Teleport();

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength); // Realiza un raycast hacia abajo para verificar si el jugador está en el suelo
        // Si el jugador está en el suelo y se ha presionado la barra espaciadora para saltar, restablece los saltos disponibles
        if (isGrounded && jumpPressed)
        {
            jumpPressed = false; // Reinicia el estado de salto después de realizar un salto
            remainingJumps = maxJumps; // Restablece los saltos disponibles al aterrizar
        }
        if (Input.GetKeyDown(KeyCode.Space) && (remainingJumps > 0 || isGrounded)) // Si el jugador está en el suelo y presiona la barra espaciadora, realiza un salto siempre que tenga saltos disponibles
        {
            if (remainingJumps > 0)
            {
                remainingJumps--;
                Jump();
            }
        }
    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement = movement.normalized;

        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
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
    // Teletransporta al jugador en la dirección actual cuando se presiona la tecla de teletransporte -DASH-
    public void Teleport()
    {
        //  Verifica si se presiona la tecla de teletransporte (por ejemplo, la tecla "E")
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Dash");
            transform.position += currentDirection * 5f; // Teletransporta al jugador en la dirección actual multiplicada por una distancia de teletransporte (ajusta el valor según sea necesario)
            Debug.Log("Current Direction: " + currentDirection); // Imprime la dirección actual en la consola para verificar que se esté calculando correctamente
        }
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
    }
}
