
using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    private float _startTime;
    private Vector3 _dashDir;
    private float _dashSpeed;
    

    public override void EnterState(PlayerController player)
    {
        Debug.Log("Iniciando Dash en State Machine");
        player.isDashing = true;
        if (AudioManager.Instance != null && AudioManager.Instance.dashSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.dashSFX);
        }
        else
        {
            Debug.LogWarning("AudioManager o dashSFX no está asignado.");
        }
        _startTime = Time.time;

        // --- NUEVO CÁLCULO DE DIRECCIÓN (IGUAL AL MOVE) ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 forward = player.camTransform.forward;
        Vector3 right = player.camTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        _dashDir = forward * v + right * h;

        // Si no estás tocando ninguna tecla, que lo haga hacia adelante
        if (_dashDir.sqrMagnitude < 0.001f) _dashDir = player.visualRoot.transform.forward;

        _dashDir.Normalize();
        // --------------------------------------------------
        // DESACTIVAR GRAVEDAD Y APLICAR VELOCIDAD INICIAL
        player.rb.useGravity = false;
        player.rb.linearVelocity = Vector3.zero;
        // Calculamos la velocidad necesaria para cubrir la distancia del dash en el tiempo deseado
        float dist = player.dashSettings.dashDistance;
        // Aplicamos velocidad bruta de inmediato
        _dashSpeed = dist / player.dashSettings.dashDuration; // Usamos el tiempo fijo que pusiste para probar
        player.rb.linearVelocity = _dashDir * _dashSpeed;

        player.dashSettings.canDashInAir = false; // Permitir dash en el aire (puedes ajustar esto según tus necesidades)

        if (player.ghostEffect != null) player.ghostEffect.StartTrail();
        // SEGURIDAD: Solo intentamos la animación si existe en el Animator
        if (HasState(player.animator, "a_Dashing"))
        {
            player.animator.CrossFade("a_Dashing", 0.05f);
        }
        else
        {
            Debug.LogWarning("Animación 'a_Dashing' no encontrada. Se usará el GhostEffect como feedback principal.");
        }
    }
    public override void UpdateState(PlayerController player)
    {
        // Forzamos 0.5f a mano para probar
        if (Time.time >= _startTime + 0.5f)
        {
            EndDash(player);
            CheckSwitchState(player);
        }
    }

    private void EndDash(PlayerController player)
    {
        player.rb.linearVelocity = Vector3.zero;
        player.rb.useGravity = true;

        // DESACTIVAR EL RASTRO
        if (player.ghostEffect != null)
        {
            player.ghostEffect.StopTrail();
        }

        player.isDashing = false;
        
    }
    public override void CheckSwitchState(PlayerController player)
    {
        float moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).sqrMagnitude;
        if (moveInput > 0.01f) player.SwitchState(player.walkingState);
        else player.SwitchState(player.idleState);
    }

    private bool HasState(Animator animator, string stateName)
    {
        if (animator == null) return false;
        return animator.HasState(0, Animator.StringToHash(stateName));
    }
}
