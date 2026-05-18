using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public bool isMoving; // Indica si el jugador está actualmente moviéndose para controlar la animación de movimiento
    public override void EnterState(PlayerController player)
    {
               Debug.Log("Player está en Walking State");
       player.animator.CrossFade("a_Walking", 0.1f); // Reproduce la animación de caminar con una transición suave.
    }
    public override void UpdateState(PlayerController player)
    {
        //Aqui chequeamos si hay que cambiar de estado, por ejemplo, si el jugador deja de presionar la tecla de movimiento, podríamos cambiar al estado de idle.
        HandleGlobalInputs(player);

        if (player.currentState != this) return;
        bool isMoving = Move(player);
        if (!isMoving)
        {
            player.SwitchState(player.idleState);
            return; // Siempre salir después de un SwitchState
        }

        HandleFallingAndLanding(player);
    }


    public override void CheckSwitchState(PlayerController player)
    {
    }
}
