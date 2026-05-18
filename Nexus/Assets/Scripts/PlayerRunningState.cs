using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player está en Running State");
       // player.animator.CrossFade("a_Running", 0.1f); // Reproduce la animación de correr con una transición suave.
    }
    public override void UpdateState(PlayerController player)
    {
        //HandleGlobalInputs(player);
        //CheckSwitchState(player);
    }
    public override void CheckSwitchState(PlayerController player)
    {
    }
}
