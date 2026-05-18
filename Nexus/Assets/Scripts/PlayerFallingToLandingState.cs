using UnityEngine;

public class PlayerFallingToLandingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player está en FallingToLanding State");
        player.animator.CrossFade("Falling To Landing", 0.1f); // Reproduce la animación de caer a aterrizar con una transición suave.
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
        // Aquí podríamos verificar si la animación de caer a aterrizar ha terminado para cambiar al estado de idle o walking.
        if (player.animator.GetCurrentAnimatorStateInfo(0).IsName("Falling To Landing") &&
            player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // La animación ha terminado, cambiamos al estado de idle o walking según corresponda.
            float input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).sqrMagnitude;

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
