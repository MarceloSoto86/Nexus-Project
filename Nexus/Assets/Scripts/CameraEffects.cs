using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
public class CameraEffects : MonoBehaviour
{
    public Transform cameraTransform; // Referencia al transform de la cámara
    //public float shakeDuration = 0.5f; // Duración del efecto de sacudida
    //public float shakeForce; // Magnitud del efecto de sacudida
    public float shakeIncreasinglMagnitude = 5f; // Magnitud inicial del efecto de sacudida para restaurarla después de cada sacudida
    public float shakeIntensityMultiplier = 0.5f; // Multiplicador para ajustar la intensidad de la sacudida en función de la memoria del jugador
    public float shakeFrequency = 0.1f; // Frecuencia de la sacudida (cuántas veces por segundo se aplica el efecto)
    public Slider _sliderMemory; // Referencia al slider de memoria para mostrar el efecto de sacudida en el HUD
    public CinemachineImpulseSource _impulseSource; // Referencia al componente CinemachineImpulseSource para generar impulsos en la cámara

    //private PlayerController _playerController; // Referencia al script PlayerController para acceder a su estado y funciones
    //private Vector3 originalCameraPosition; // Posición original de la cámara antes de aplicar el efecto de sacudida
    private float nextShakeTime; // Tiempo en el que se aplicará la próxima sacudida



    private void Start()
    {
        if (_impulseSource == null)
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>(); // Obtiene la referencia al componente CinemachineImpulseSource si no se ha asignado en el inspector
        }

    }

    private void Update()
    {
        if (_sliderMemory.value <= 0.2f)
        {
            GenerateImpulseOnCamera(); // Genera un impulso en la cámara cuando la memoria del jugador es baja (20% o menos)
        }
    }

    public void GenerateImpulseOnCamera()
    {
        if (_impulseSource == null) return; // Si no hay un componente CinemachineImpulseSource asignado, no se puede generar el impulso

        float intensity = (shakeIntensityMultiplier - _sliderMemory.value) * shakeIncreasinglMagnitude; // Calcula la intensidad del impulso en función de la memoria del jugador, aumentando a medida que la memoria disminuye

        _impulseSource.GenerateImpulseWithVelocity(Random.insideUnitSphere * intensity); // Genera un impulso en la cámara con una dirección aleatoria y una magnitud basada en la intensidad calculada
    }
}
