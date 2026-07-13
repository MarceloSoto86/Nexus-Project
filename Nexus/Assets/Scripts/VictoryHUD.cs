using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryHUD : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    private void OnEnable()
    {
        VictoryTrigger.OnPlayerVictory += ShowVictoryHUD;
    }

    private void OnDisable()
    {
        VictoryTrigger.OnPlayerVictory -= ShowVictoryHUD;
    }

    private void ShowVictoryHUD()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego

            Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
            Cursor.visible = true; // Hace visible el cursor
        }
        else
        {
            //Debug.logWarning("Victory Panel is not assigned in the inspector.");
        }
    }

    public void ReturnToMainMenuButton()
    {
        //Descongelamos el tiempo antes de cambiar de escena
        Time.timeScale = 1f; // Reanuda el juego antes de cambiar de escena
        SceneManager.LoadScene("Main Menu"); // Asegúrate de que el nombre de la escena sea correcto
    }
}