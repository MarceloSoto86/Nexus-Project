using UnityEngine;

public class PlayerFlashingDamageState : PlayerBaseState
{
    private float _flashDuration = 0.2f; // Duración del parpadeo de daño, ajusta según tu animación
    private float _startTime;
    

    public override void EnterState(PlayerController player)
    {
            Debug.Log("Player está en Flashing Damage State");
        player.animator.CrossFade("Getting_damage", 0.1f);
        player.rb.linearVelocity = new Vector3(0f, player.rb.linearVelocity.y, 0f); // Detener el movimiento horizontal
        _startTime = Time.time;

    }
    public override void UpdateState(PlayerController player)
    {

        if (player.currentState != this) return;
        //CHEQUEO DEL TIEMPO (Siempre corre en paralelo)
        if(Time.time >= _startTime + _flashDuration)
        {
            // El tiempo de parpadeo ha terminado, volvemos al estado de idle o walking según corresponda.
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
    public override void CheckSwitchState(PlayerController player)
    {
    }

}
