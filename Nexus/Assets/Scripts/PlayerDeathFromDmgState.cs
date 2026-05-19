using UnityEngine;

public class PlayerDeathFromDmgState : PlayerBaseState
{
    private float _deathStartTime;
    private float _deathDuration = 5f; // Duración de la animación de muerte, ajusta según tu animación
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player está en Dying from Damage State");
        player.rb.linearVelocity = new Vector3(0f, player.rb.linearVelocity.y, 0f); // Detener el movimiento horizontal
        player.animator.CrossFade("Dying_from_damage", 0.1f);
        _deathStartTime = Time.time;
    }
    public override void UpdateState(PlayerController player)
    {
       
        if (player.currentState != this) return;

        //CHEQUEO DEL TIEMPO (Siempre corre en paralelo)
        bool timeIsUp = Time.time >= _deathStartTime + _deathDuration;

        // Si el tiempo de la animación de muerte ha terminado, reiniciamos el estado del jugador y lo respawneamos
        if (timeIsUp)
        {
            player.GetComponent<PlayerStatus>().ResetStatus(); // Reiniciar el estado del jugador (vida, energía, etc.)
            player.rb.isKinematic = false; // Reactivar la física para el jugador
            player.Respawn(); // Llama al método de respawn del jugador después de que termine la animación de muerte
            player.SwitchState(player.idleState); // Cambia al estado de idle después de respawnear
            return;
        }

        if (player.IsGrounded())
        {
            player.rb.isKinematic = true; // Detener la física para evitar que el jugador se mueva o caiga
        }
        
        // Aquí podríamos agregar lógica para esperar a que termine la animación de muerte, o para reiniciar el nivel, etc.
    }
    public override void CheckSwitchState(PlayerController player)
    {
    }
}
