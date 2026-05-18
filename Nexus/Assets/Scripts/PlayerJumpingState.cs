using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
   public override void EnterState(PlayerController player)
    {
        Debug.Log("Player est� en Jumping State");
        player.remainingJumps--;

        // Antes de aplicar la fuerza de salto, restablece la velocidad vertical del jugador a cero para evitar que el salto se vea afectado por la velocidad actual
        Vector3 velocity = player.rb.linearVelocity;
        velocity.y = 0f;
        player.rb.linearVelocity = velocity;

        // Aplica una fuerza hacia arriba para realizar el salto
        player.rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
        player.animator.CrossFade("Jump", 0.1f); // Reproduce la animación de salto con una transición suave

        player.jumpPressed = true; // Establece el estado de salto para evitar que el jugador pueda saltar nuevamente hasta que aterrice
        player.nextGroundCheckTime = Time.time + player.groundCheckDelay; // Establece el tiempo para el próximo chequeo de suelo después de realizar un salto
        // Al hacer SwitchState al mismo estado, se vuelve a ejecutar EnterState y se aplica la fuerza de salto nuevamente
    }
    public override void UpdateState(PlayerController player)
    {
        HandleGlobalInputs(player);
        Move(player);
        // Forzamos que el visualRoot no se desplace lateralmente por la animación
        if (player.visualRoot != null)
        {
            player.visualRoot.transform.localPosition = Vector3.zero;
        }

        HandleFallingAndLanding(player);
        CheckSwitchState(player);
    }
    public override void CheckSwitchState(PlayerController player)
    {
       
    }
}
