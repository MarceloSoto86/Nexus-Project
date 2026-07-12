using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private float footstepTimer; // Temporizador para controlar la frecuencia de los pasos
    [SerializeField] private float timeBetweenSteps = 0.4f; // Ajustable según el ritmo de caminata

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

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                if (AudioManager.Instance != null && AudioManager.Instance.footstepSFX != null)
                {
                    // Reproducimos el paso con un volumen ligeramente más bajo para que no aturda
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.footstepSFX, 0.5f);
                }
                footstepTimer = timeBetweenSteps; // Reseteamos el temporizador
            }
        }

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
