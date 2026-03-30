using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float xAxis = 0f;
    [SerializeField] private float yAxis = 0f;
    [SerializeField] private float zAxis = 0f;
    [SerializeField] Transform _player;
    private Vector3 _currentVelocity; // Variable para almacenar la velocidad actual de la cámara
    public float smoothSpeed = 0.125f; // Velocidad de suavizado para el movimiento de la cámara
    public Vector3 offset; // Offset para la posición de la cámara con respecto al jugador


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 offsetPos = _player.position + offset; // Establece la posición de la cámara con un offset respecto al jugador - _player.position + new Vector3(xAxis, yAxis, zAxis);
        transform.position = Vector3.SmoothDamp(transform.position, offsetPos, ref _currentVelocity, smoothSpeed); // Suaviza el movimiento de la cámara hacia la posición objetivo utilizando SmoothDamp
        //transform.LookAt(_player.position + (_player.position.y + 1));
    }
}
