using UnityEngine;

public class MemorySerum : MonoBehaviour
{
    public string uniqueID; 
    public float restoreMemoryAmount = 20f; // Cantidad de memoria que el suero de memoria restaurará
    public bool unlockNextMemorySlot = true; // Indica si el suero de memoria desbloquea el siguiente slot de memoria
    public GameObject collectEffect; // Referencia al efecto de recolección para mostrarlo en el HUD cuando el jugador recolecta memoria o salud para indicar visualmente

    private void Start()
    {
        PlayerData data = SaveSystem.instance.LoadGame(); // Carga los datos guardados del jugador al iniciar el juego
        if(data != null && data.collectedItemIDs.Contains(uniqueID))
        {
            Destroy(gameObject); // Destruye el objeto del suero de memoria si ya ha sido recolectado anteriormente
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponentInParent<PlayerStatus>();
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

                if (collectEffect != null)
                {
                    Instantiate(collectEffect, transform.position, Quaternion.identity); // Instancia el efecto de recolección en la posición del suero de memoria
                }
                playerStatus.AddCollectedItemID(uniqueID);

                if (AudioManager.Instance != null && AudioManager.Instance.collectSerumSFX != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.collectSerumSFX, 0.5f); // Reproduce el efecto de sonido de recolección del suero de memoria
                }
                //SaveSystem.instance.SaveGame(playerStatus); // Guarda el estado del jugador después de recoger el suero de memoria
                Destroy(gameObject); // Destruye el objeto del suero de memoria después de recogerlo
            }
        }

        
    }

    
}
