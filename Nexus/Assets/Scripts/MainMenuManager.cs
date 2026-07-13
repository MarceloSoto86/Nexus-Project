using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public List<GameObject> settingsContent;
    public Button continueGameButton;
    //public List<GameObject> settingsButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (continueGameButton != null)
        { 

             if(SaveSystem.instance != null && System.IO.File.Exists(SaveSystem.instance.filePath))
             {
                 continueGameButton.interactable = true; // Habilita el botón de "Continuar" si existe un archivo de guardado
             }
             else
             {
                 continueGameButton.interactable = false; // Deshabilita el botón de "Continuar" si no existe un archivo de guardado
             }
        }  
        else 
        {
            //Debug.logWarning("Continue Game button is not assigned in the inspector.");
        }
        ChangeTab(0);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ChangeTab(int index)
    {
        for (int i = 0; i < settingsContent.Count; i++)
        {
            if (i == index)
            {
                settingsContent[i].SetActive(true);
               // settingsButtons[i].SetActive(true);
               if (settingsContent[i].name == "AudioContent")
                {
                    // Aquí puedes agregar código específico para configurar los controles de audio, como ajustar el volumen o mostrar opciones relacionadas con el audio.
                    //Debug.log("Audio Settings tab selected");
                }
               else if (settingsContent[i].name == "GraphicsContent")
                {
                    // Aquí puedes agregar código específico para configurar los controles de video, como ajustar la resolución o mostrar opciones relacionadas con el video.
                    //Debug.log("Video Settings tab selected");
                }
                else if (settingsContent[i].name == "GameContent")
                {
                    // Aquí puedes agregar código específico para configurar los controles de juego, como asignar teclas o mostrar opciones relacionadas con los controles.
                    //Debug.log("Controls Settings tab selected");
                }
            }
            else
            {
                settingsContent[i].SetActive(false);
               // settingsButtons[i].SetActive(false);
            }
        }
    }

    public void StartNewGame()
    {
        if(SaveSystem.instance != null)
        {
            SaveSystem.instance.DeleteSavedData(); // Elimina el archivo de guardado existente al iniciar un nuevo juego para asegurarse de que el jugador comience desde cero sin cargar un progreso anterior.
        }
        SceneManager.LoadScene("Main Level");
    }

    public void ContinueGame()
    {
        if(SaveSystem.instance != null && System.IO.File.Exists(SaveSystem.instance.filePath))
        {
            SceneManager.LoadScene("Main Level");
        }
        else
        {
            //Debug.logWarning("No saved game found. Please start a new game.");
        }
    }
    public void QuitGame()
    {
        Application.Quit();
        //Debug.log("Quit Game");
    }
}
