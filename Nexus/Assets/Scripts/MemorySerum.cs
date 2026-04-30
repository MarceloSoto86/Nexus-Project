using UnityEngine;

public class MemorySerum : MonoBehaviour
{
    public float restoreMemoryAmount = 20f; // Cantidad de memoria que el suero de memoria restaurará
    public bool unlockNextMemorySlot = true; // Indica si el suero de memoria desbloquea el siguiente slot de memoria


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                if (restoreMemoryAmount > 0)
                    {
                    playerStatus.RestoreMemory(restoreMemoryAmount);// Restaura la memoria del jugador
                    }
                if (unlockNextMemorySlot)
                { 
                    playerStatus.UnlockNextMemorySlot(); // Desbloquea el siguiente slot de memoria
                }
                Destroy(gameObject); // Destruye el objeto del suero de memoria después de recogerlo
            }
        }
    }

    
}
