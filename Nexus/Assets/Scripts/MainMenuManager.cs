using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public List<GameObject> settingsContent;
    //public List<GameObject> settingsButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                    Debug.Log("Audio Settings tab selected");
                }
               else if (settingsContent[i].name == "GraphicsContent")
                {
                    // Aquí puedes agregar código específico para configurar los controles de video, como ajustar la resolución o mostrar opciones relacionadas con el video.
                    Debug.Log("Video Settings tab selected");
                }
                else if (settingsContent[i].name == "GameContent")
                {
                    // Aquí puedes agregar código específico para configurar los controles de juego, como asignar teclas o mostrar opciones relacionadas con los controles.
                    Debug.Log("Controls Settings tab selected");
                }
            }
            else
            {
                settingsContent[i].SetActive(false);
               // settingsButtons[i].SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Level");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
