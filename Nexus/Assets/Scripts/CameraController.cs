using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private float xAxis = 0f;
    //[SerializeField] private float yAxis = 0f;
    //[SerializeField] private float zAxis = 0f;
    [SerializeField] Transform _player;
    private Vector3 _currentVelocity; // Variable para almacenar la velocidad actual de la cámara
    private Quaternion _targetRotation; // Variable para almacenar la rotación objetivo de la cámara
    public Vector3 _targetOffset; // Variable para almacenar el offset objetivo de la cámara

    [Header("Configuración de Seguimiento")]
    public float smoothSpeed = 0.125f; // Velocidad de suavizado para el movimiento de la cámara
    //public Vector3 offset; // Offset para la posición de la cámara con respecto al jugador
    public float rotationSpeed = 5f; // Velocidad de suavizado para la rotación de la cámara


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _targetOffset = _targetOffset; // Inicializa el offset objetivo con el valor configurado en el inspector
        _targetRotation = transform.rotation; // Inicializa la rotación objetivo con la rotación actual de la cámara
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 offsetPos = _player.position + _targetOffset; // Establece la posición de la cámara con un offset respecto al jugador - _player.position + new Vector3(xAxis, yAxis, zAxis);
        transform.position = Vector3.SmoothDamp(transform.position, offsetPos, ref _currentVelocity, smoothSpeed); // Suaviza el movimiento de la cámara hacia la posición objetivo utilizando SmoothDamp
        //transform.LookAt(_player.position + (_player.position.y + 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime); // Suaviza la rotación de la cámara hacia la rotación objetivo utilizando Slerp
    }

    public void ChangeView(Vector3 newOffset, Vector3 newRotation) //   Esta función se llama desde el script CameraZone para cambiar la vista de la cámara
    {
        _targetOffset = newOffset; // Actualiza el offset para la nueva vista
        _targetRotation = Quaternion.Euler(newRotation); // Establece la rotación objetivo para la nueva vista

    }
}
