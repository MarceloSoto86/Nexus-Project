using UnityEngine;
using System;

public class VictoryTrigger : MonoBehaviour
{
 public static event Action OnPlayerVictory;

    private void OnTriggerEnter(Collider other)
    {
        //Verificamos si el objeto que ha entrado en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=green>¡VICTORIA REAL ACTIVADA!</color>");
            Debug.Log("Player has reached the victory trigger!");


            // Invocamos el evento de victoria del jugador si hay suscriptores
            if (OnPlayerVictory != null)
            {
                OnPlayerVictory.Invoke();
            }
        }
    }

}
