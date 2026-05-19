using UnityEngine;

public class PlayerDeathFromInsanityState : PlayerBaseState
{
    private float _deathStartTime;
    private float _agonyDuration = 3f; // Duración de la animación de agonía, ajusta según tu animación
    private float _deathDuration = 5f; // Duración de la animación de muerte, ajusta según tu animación
    private bool _hasCollapsed = true;
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player está en PlayerDeathFromInsanityState");
        player.rb.linearVelocity = new Vector3(0f, player.rb.linearVelocity.y, 0f); // Detener el movimiento horizontal
        player.animator.CrossFade("Agony", 0.1f);
        _deathStartTime = Time.time;
        _hasCollapsed = false; // Reiniciamos el flag para asegurarnos de que la caída solo ocurra una vez
    }

    public override void UpdateState(PlayerController player)
    {
       bool agonyIsOver = Time.time >= _deathStartTime + _agonyDuration;
        if (agonyIsOver && !_hasCollapsed)
        {
            player.animator.CrossFade("Collapsing", 0.1f);
            _hasCollapsed = true; // Marcamos que el jugador ya ha colapsado para no repetir la animación
            //_deathStartTime = Time.time; // Reiniciamos el tiempo para contar la duración de la animación de muerte
        }

        if (player.IsGrounded() && _hasCollapsed == true)
        {
            player.rb.isKinematic = true;
        }

        if (Time.time >= _deathStartTime + _agonyDuration + _deathDuration)
        {
            player.GetComponent<PlayerStatus>().ResetStatus(); // Reiniciar el estado del jugador (vida, energía, etc.)
            player.rb.isKinematic = false; // Reactivar la física para el jugador
            player.Respawn(); // Llama al método de respawn del jugador después de que termine la animación de muerte
            player.SwitchState(player.idleState); // Cambia al estado de idle después de respawnear
            return;
        }

    }

    public override void CheckSwitchState(PlayerController player)
    {
        
    }
}
