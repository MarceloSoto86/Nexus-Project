using UnityEngine;
using UnityEngine.UI;

public class DashSkillUnlockTrigger : MonoBehaviour
{
    [SerializeField] private MemoryFlashes dashMemoryData; // Reference to the MemoryFlashes script
    [SerializeField] private Image dashIconImage; // Reference to the UI Image for the memory flash

    private void Start()
    {
        if (dashIconImage != null)
        {
            dashIconImage.enabled = false; // Ensure the dash icon is initially disabled
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Unlock the dash skill in the player's controller
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.isDashUnlocked = true;
                Debug.Log("<color=cyan>[NEXUS SYSTEM]: Habilidad Dash Activada de forma permanente.</color>");
            }

            if (playerController.dashHUDIcon != null)
            {
                playerController.dashHUDIcon.enabled = true; // Enable the dash icon in the UI
            }

            if (MemoryManager.Instance != null && dashMemoryData != null)
            {
                MemoryManager.Instance.TriggerMemory(dashMemoryData);
            }

            GetComponent<Collider>().enabled = false; // Disable the trigger to prevent re-triggering
            //Destroy(gameObject, 0.1f); // Destroy the trigger after 0.1 seconds to allow the player to see the memory flash

        }
    }
}
