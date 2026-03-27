using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rayLength = 1.1f; 
    public int remainingJumps = 2;
    public int maxJumps = 2;
    private Rigidbody rb;
    public static PlayerController player;
    public bool jumpPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength); // Realiza un raycast hacia abajo para verificar si el jugador está en el suelo

        if (isGrounded && jumpPressed)
        {
            jumpPressed = false; // Reinicia el estado de salto después de realizar un salto
            remainingJumps = maxJumps; // Restablece los saltos disponibles al aterrizar
        }
            if(Input.GetKeyDown(KeyCode.Space) && (remainingJumps >0 || isGrounded)) // Si el jugador está en el suelo y presiona la barra espaciadora, realiza un salto siempre que tenga saltos disponibles
            {
            if(remainingJumps > 0)
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

    public void Jump()
    { 
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpPressed = true; // Establece el estado de salto para evitar que el jugador pueda saltar nuevamente hasta que aterrice
    }
}
