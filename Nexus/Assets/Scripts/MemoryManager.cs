using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MemoryManager : MonoBehaviour
{
    public Image _displayImageUI;
    public Image _faceHUDImageUI;
    public TextMeshProUGUI _textMeshPro;
    public CanvasGroup _canvasGroup;
    

    public float fadeInDuration = 1f; // Duración del fade in
    public float displayDuration = 2f; // Duración de la imagen visible
    public float fadeOutDuration = 1f; // Duración del fade out

IEnumerator Fade(MemoryFlashes data)
    {         
        Time.timeScale = 0f; // Pausa el juego
        PlayMemory(data); // Reproduce el recuerdo con la información del ScriptableObject
        // Aseguramos que el contenedor esté activo (aunque el alpha sea 0)
        _displayImageUI.gameObject.SetActive(true);
        _textMeshPro.gameObject.SetActive(true);
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 1f; // Asegura que la imagen esté completamente visible
        //_textMeshPro.text = "Recuerdo: " + _imageMemory.name; // Muestra el nombre del sprite como texto. Modificar el texto para mostrar la información que deseas sobre el recuerdo, como una descripción o un mensaje relacionado con la imagen.
        // Display duration
        yield return new WaitForSecondsRealtime(displayDuration); // Mantiene la imagen visible durante el tiempo especificado
        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0f; // Asegura que la imagen esté completamente invisible
        Time.timeScale = 1f; // Reanuda el juego
    }
    // Start es llamado antes de la primera actualización del frame después de que el MonoBehaviour es creado y el objetivo de este método es inicializar cualquier dato o estado necesario para el script. En este caso, se obtiene la referencia al componente TextMeshPro y CanvasGroup, y se inicia la corrutina de fade.
    private void Start()
    {
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    public void TriggerMemory(MemoryFlashes data)
    {
        StartCoroutine(Fade(data)); // Llama a la corrutina de fade para mostrar el recuerdo cuando se active el trigger.
    }
    public void PlayMemory(MemoryFlashes data)
    {
        if(data == null || data._nitidFaceImg == null || data._imageMemory == null)
        {
            Debug.LogWarning("MemoryFlashes data is null. Cannot play memory.");
            return; // Sale del método si el ScriptableObject es nulo para evitar errores.
        }
        _displayImageUI.sprite = data._imageMemory; // Asigna la imagen del recuerdo a la variable spriteToShow para mostrarla en el HUD.
        //spriteToShow.sprite = data.memorySprite; // Cambia el sprite de la imagen del recuerdo al sprite especificado en el ScriptableObject.
        Debug.Log("Playing memory: " + _displayImageUI.sprite.name); // Imprime el nombre del sprite en la consola para verificar que se está reproduciendo el recuerdo correcto.
    }

    public void ChangeFaceNitid(MemoryFlashes data)
    {
        if(data == null || data._nitidFaceImg == null)
        {
            Debug.LogWarning("MemoryFlashes data is null. Cannot change face nitid.");
            return; // Sale del método si el ScriptableObject es nulo para evitar errores.
        }
        _faceHUDImageUI.sprite = data._nitidFaceImg; // Cambia la imagen del HUD de la cara nitida a la imagen del recuerdo que se acaba de mostrar.
        Debug.Log("Cara nitida desbloqueada!"); // Imprime un mensaje en la consola para verificar que se ha desbloqueado la cara nitida.
    }
}
