using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MemorySlotsHUD : MonoBehaviour
{
    private Slider _mainSlider;

    [Header("Memory Slot Icons")]
    [SerializeField] private Image[] memorySlotIcons; // Array de imágenes para los iconos de los slots de memoria
    [SerializeField] private Color activeSlotColour = Color.magenta; // Color para los slots de memoria llenos
    [SerializeField] private Color lockedSlotColour = new Color(0.2f, 0.2f, 0.2f, 0.5f); // Color para los slots de memoria bloqueados

    private float _previousUnlockedCount; // Variable para almacenar la cantidad de slots de memoria desbloqueados en la actualización anterior para evitar actualizaciones innecesarias del HUD de memoria si la cantidad de slots de memoria desbloqueados no ha cambiado desde la última actualización, lo que mejora el rendimiento al reducir la cantidad de cálculos y actualizaciones visuales que se realizan cada vez que se actualiza el HUD de memoria
    private float _maxPointsPerSlot = 25f; // Variable para almacenar la cantidad máxima de puntos por slot de memoria

    private void Awake()
    {
        _mainSlider = GetComponent<Slider>(); // Obtener la referencia al componente Slider del objeto para actualizar su valor en función de la memoria actual del jugador
        _previousUnlockedCount = 1; // Inicializar la cantidad de slots de memoria desbloqueados en la variable _previousUnlockedCount a 1 al inicio del juego para reflejar que el jugador comienza con un slot de memoria desbloqueado y evitar actualizaciones innecesarias del HUD de memoria si la cantidad de slots de memoria desbloqueados no ha cambiado desde la última actualización
    }

    public void RefreshSanityHUD(float currentSanity, int unlockedSlotsCount)
    {
        if (_mainSlider != null)
        {
            _mainSlider.value = currentSanity; // Actualizar el valor del slider principal de memoria en función de la cordura actual del jugador para mostrar al jugador su estado de memoria actual de manera visual a través del slider
        }
        UpdateSlotsDisplay(currentSanity, unlockedSlotsCount); // Llamar al método UpdateSlotsDisplay para actualizar la visualización de los slots de memoria en función de la cordura actual del jugador y la cantidad de slots de memoria desbloqueados
        if(unlockedSlotsCount > _previousUnlockedCount)
        {
            if(_previousUnlockedCount >= 1 && currentSanity > _maxPointsPerSlot)
            {
                StartCoroutine(AnimateSlotUnlock(unlockedSlotsCount - 1)); // Iniciar la corrutina AnimateSlotUnlock para animar el desbloqueo del nuevo slot de memoria desbloqueado restando 1 al nuevo valor de unlockedSlotsCount para reflejar el cambio en la cantidad de slots de memoria desbloqueados y mostrar al jugador una animación visual que indique que ha desbloqueado un nuevo slot de memoria
            }
            _previousUnlockedCount = unlockedSlotsCount; // Actualizar la cantidad de slots de memoria desbloqueados en la variable _previousUnlockedCount restando 1 al nuevo valor de unlockedSlotsCount para reflejar el cambio en la cantidad de slots de memoria desbloqueados y evitar actualizaciones innecesarias del HUD de memoria si la cantidad de slots de memoria desbloqueados no ha cambiado desde la última actualización
        }


    }
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        PlayerEvents.OnSanityChanged += RefreshSanityHUD; // Suscribirse al evento de cambio de cordura del jugador para actualizar el HUD de memoria cada vez que la cordura cambie, lo que permite que el HUD refleje con precisión el estado de memoria actual del jugador en tiempo real
    }

    public void UpdateSlotsDisplay(float currentSanity, int unlockedSlotsCount)
    {
        for (int i = 0; i < memorySlotIcons.Length; i++)
        {
            if (i>= unlockedSlotsCount)
            {
                memorySlotIcons[i].color = lockedSlotColour;
                memorySlotIcons[i].fillAmount = 0f; // Si el slot de memoria está bloqueado, establecer su cantidad de relleno a 0 para mostrarlo como vacío
                continue; // Saltar a la siguiente iteración del bucle para no actualizar el slot de memoria bloqueado
            }

            memorySlotIcons[i].color = activeSlotColour; // Si el slot de memoria está desbloqueado, establecer su color al color de slot activo para mostrarlo como disponible para usar

            float slotFloor = i * _maxPointsPerSlot; // Calcular el piso del slot de memoria actual multiplicando el índice del slot de memoria por la cantidad máxima de puntos por slot de memoria para determinar cuánta cordura se necesita para llenar completamente el slot de memoria actual

            if (currentSanity > slotFloor)
            {
                float sanityInThisSlot = Mathf.Clamp(currentSanity - slotFloor, 0f, _maxPointsPerSlot); // Calcular la cantidad de cordura en este slot de memoria restando el piso del slot de memoria a la cordura actual del jugador y limitándola entre 0 y la cantidad máxima de puntos por slot de memoria para evitar que se muestre una cantidad mayor a la capacidad del slot de memoria

                memorySlotIcons[i].fillAmount = Mathf.Clamp01(sanityInThisSlot / _maxPointsPerSlot); // Actualizar la cantidad de relleno del icono del slot de memoria en función de la cantidad de cordura en este slot de memoria dividida por la cantidad máxima de puntos por slot de memoria para mostrar al jugador cuánto del slot de memoria está lleno de manera visual a través del relleno del icono
            }
            else
            {
                memorySlotIcons[i].fillAmount = 0f; // Si la cordura actual del jugador es menor o igual al piso del slot de memoria, establecer la cantidad de relleno del icono del slot de memoria a 0 para mostrarlo como vacío
            }


        }
    }
    private void OnDisable()
    {
        // Súper importante: Rompemos el lazo al desactivar el objeto para no saturar la memoria RAM
        PlayerEvents.OnSanityChanged -= RefreshSanityHUD;
    }

    IEnumerator AnimateSlotUnlock(int slotIndex)
    {
        float animationDuration = 0.05f; // Duración de la animación en segundos
        float elapsedTime = 0f;
        float shrinkDuration = 0.1f; // Duración de la animación de encogimiento en segundos

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Aquí puedes agregar la lógica de animación, por ejemplo, cambiar el color o el tamaño del icono del slot de memoria
            memorySlotIcons[slotIndex].color = Color.Lerp(lockedSlotColour, activeSlotColour, t);
            memorySlotIcons[slotIndex].rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, t); // Animación de escala para hacer que el icono del slot de memoria "salte" al desbloquearse
            yield return null;
        }

        elapsedTime = 0f; // Reiniciar el tiempo transcurrido para la animación de encogimiento

        while (elapsedTime < shrinkDuration)
        { 
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / shrinkDuration);
            memorySlotIcons[slotIndex].rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, t); // Animación de escala para hacer que el icono del slot de memoria "salte" al desbloquearse

            yield return null;
        }

        // Asegurarse de que el slot de memoria esté completamente desbloqueado al final de la animación
        memorySlotIcons[slotIndex].color = activeSlotColour;
        memorySlotIcons[slotIndex].transform.localScale = Vector3.one;
    }
 }