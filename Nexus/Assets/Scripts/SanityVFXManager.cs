using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityVFXManager : MonoBehaviour
{
    [Header("Post-Processing Volume Settings")]
    [SerializeField] private Volume globalVolume; //Arrastro el volumen global desde la escena

    public float sanityThreshold = 30f; // Umbral de cordura para activar los efectos visuales
    public float insanityIntensity = 1f; // Intensidad de los efectos visuales (0 a 1)
    public float maxSlotPoints = 100f; // Máxima cantidad de puntos de slot que el jugador puede tener para aumentar la intensidad de los efectos visuales

    private Vignette vignetteEffect; // Referencia al efecto de viñeta
    private ChromaticAberration chromaticAberrationEffect; // Referencia al efecto de aberración cromática

    private void Awake()
    {
        if (globalVolume == null)
        {
            //Debug.logError("Global Volume no asignado en SanityVFXManager.");
            return;
        }
        if (globalVolume != null && globalVolume.profile.TryGet(out chromaticAberrationEffect))
        {
           
        }
        if(globalVolume != null && globalVolume.profile.TryGet(out vignetteEffect))
        {

        }
    }

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de cordura del jugador
        PlayerEvents.OnSanityChanged += HandleSanityVFX;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento para evitar fugas de memoria
        PlayerEvents.OnSanityChanged -= HandleSanityVFX;
    }

    private void HandleSanityVFX(float currentSanity, int UnlockedSlotCount)
    {
        if (currentSanity < sanityThreshold) {
            ApplySanityVFX(currentSanity, UnlockedSlotCount);
        }
        else
        {
            // Si la cordura está por encima del umbral, restablecer los efectos visuales a su estado normal
            if (chromaticAberrationEffect != null)
            {
                chromaticAberrationEffect.intensity.value = 0f; // Restablece la intensidad de la aberración cromática
            }
            if (vignetteEffect != null)
            {
                vignetteEffect.intensity.value = 0.2f; // Restablece la intensidad de la viñeta a un valor mínimo
            }
        }
    }

    private void ApplySanityVFX(float currentSanity, int UnlockedSlotCount)
    { 
        //1. Calcular cual es el nivel de locura basado en la cordura actual del jugador.
        insanityIntensity = 1f - (currentSanity / maxSlotPoints); // Calcula la intensidad basada en la cordura actual

       //2. Aplicar los efectos visuales si la cordura está por debajo del umbral
       if(chromaticAberrationEffect != null)
       {
            //A menor cordura, mayor intensidad de los efectos visuales. La intensidad se ajusta según la cantidad de puntos de slot desbloqueados.
            chromaticAberrationEffect.intensity.value = Mathf.InverseLerp(sanityThreshold, 0f, currentSanity); // Ajusta la intensidad de la aberración cromática
       }

       if (vignetteEffect != null)
       {
            float dangerFactor = Mathf.InverseLerp(sanityThreshold, 0f, currentSanity); // Calcula un factor de peligro basado en la cordura actual
            //A menor cordura, los bordes de la pantalla se vuelven más oscuros. La intensidad se ajusta según la cantidad de puntos de slot desbloqueados.
            vignetteEffect.intensity.value = Mathf.Lerp(0.2f, 0.5f, dangerFactor); // Ajusta la intensidad de la viñeta
        }
    }
}
