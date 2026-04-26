using UnityEngine;
using System;

public static class SecuritySystemActivation
{
    public static event Action OnSecuritySystemActivated; // Evento que se dispara cuando el sistema de seguridad es activado

    public static void TriggerSecuritySystem()
    {
        OnSecuritySystemActivated?.Invoke(); // Dispara el evento si hay suscriptores
    }
}
