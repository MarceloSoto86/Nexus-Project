using UnityEngine;

public class PlayerPanickedFallingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
       //Debug.log("Player has entered the Panicked Falling State.");
        player.animator.CrossFade("Panicked_Falling", 0.1f);
    }

    public override void UpdateState(PlayerController player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();
        // Aplicamos un daño masivo (o letal directo si así lo deseas por diseño)
        status.TakeDamage(status.maxHealth); // Esto detonará de forma limpia el flujo que ya repara estatus y hace respawn
    }

    public override void CheckSwitchState(PlayerController player)
    {
    }
}
