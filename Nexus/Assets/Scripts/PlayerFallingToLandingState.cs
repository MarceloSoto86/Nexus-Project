using UnityEngine;

public class PlayerFallingToLandingState : PlayerBaseState
{
    
    public override void EnterState(PlayerController player)
    {
        //Debug.log("Player está en FallingToLanding State");
        player.animator.CrossFade("a_FallingToLanding", 0.1f); // Reproduce la animación de caer a aterrizar con una transición suave.

        if (player.rb != null)
        {
            // Detenemos la velocidad vertical del jugador para que no siga cayendo durante la animación.
            Vector3 velocity = player.rb.linearVelocity;
            velocity.y = 0f;
            player.rb.linearVelocity = velocity;
        }
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
        
        if (player != null)
        {
            if (player.IsGrounded(player.rayLength))
             {
                float input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).sqrMagnitude;

                // Si hay teclado, vamos a caminar; si no, a descansar
                if (input > 0.01f)
                {
                    player.SwitchState(player.walkingState);
                }
                else
                {
                    player.SwitchState(player.idleState);
                }
             }
        }
            
           
    }


}
