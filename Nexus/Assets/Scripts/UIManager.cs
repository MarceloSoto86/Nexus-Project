using TMPro;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    //Singleton para facilitar el acceso desde otros scripts
    public static UIManager Instance;

    [Header("Memory Message UI")]
    public GameObject panelMemory;
    public TextMeshProUGUI textMemory;

    private Coroutine currentMemoryCoroutine;

    private void Awake()
    {
        //Aseguramos que solo haya una instancia de UIManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Al empezar, ocultamos el panel de memoria
        if (panelMemory != null)
        {
            panelMemory.SetActive(false);
        }
    }

    public void OpenMemoryCanvas(string narrativeText)
    {
        //Si ya hay un mensaje de memoria mostrándose, lo detenemos antes de mostrar el nuevo
        if (panelMemory != null && textMemory != null)
        {
            textMemory.text = narrativeText;
            panelMemory.SetActive(true);
        }
    }

    public void CloseMemoryCanvas()
    {
        if (panelMemory != null)
        {
            panelMemory.SetActive(false);
        }
    }
}
