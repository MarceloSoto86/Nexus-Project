using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        //Debug.log("Player está en Idle State");
        
        // FRENADO QUIRÚRGICO: 
        // Mantenemos la velocidad en Y (por si aterrizó de un salto) 
        // pero reseteamos X y Z a cero inmediatamente.
        Vector3 stopVelocity = player.rb.linearVelocity;
        stopVelocity.x = 0f;
        stopVelocity.z = 0f;
        player.rb.linearVelocity = stopVelocity;
        player.animator.CrossFadeInFixedTime("a_Idle", 0.1f,0); // Reproducimos la animación de idle cuando entramos en este estado.
                                                   // player.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, player.GetComponent<Rigidbody>().linearVelocity.y, 0);
    }

    public override void UpdateState(PlayerController player)
    {
       HandleGlobalInputs(player);
        //Aqui chequeamos si hay que cambiar de estado, por ejemplo, si el jugador presiona una tecla de movimiento, podríamos cambiar al estado de caminar o correr.
        if (player.currentState != this) return;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            player.SwitchState(player.walkingState);
            return;
        }
    
    }

    public override void CheckSwitchState(PlayerController player)
    {
    }
}
