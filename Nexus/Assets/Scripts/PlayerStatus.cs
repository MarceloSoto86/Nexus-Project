using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int _memoryMax = 100; // Valor máximo de memoria del jugador
    public int _memorySlot = 4; // Número de slots de memoria ocupados (puede ser un valor entre 0 y 10, por ejemplo)
    public int _currentSlot; // Número de slots de memoria actualmente ocupados (puede ser un valor entre 0 y _memorySlot)
    public int _memoryPerSlot = 25; // Cantidad de memoria que cada slot puede contener
    public float _currentMemory; // Valor actual de memoria del jugador
    public float _memoryDecreaseRate; // Tasa a la que la memoria disminuye
    public float currentHealth; // Valor actual de salud del jugador
    public float maxHealth = 100; // Valor máximo de salud del jugador 
    public PlayerController _playerController; // Referencia al script PlayerController para acceder a su estado y funciones


    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        _currentMemory = _memoryMax;
        _memorySlot = 1; // Inicialmente, el jugador tiene un slot de memoria ocupado
    }

    public void Update()
    {
        _currentMemory -= (_memoryDecreaseRate * Time.deltaTime); // Disminuye la memoria actual según la tasa de disminución
        _currentMemory = Mathf.Clamp(_currentMemory, 0, _memoryPerSlot * _memorySlot); // Asegura que la memoria actual no exceda el máximo permitido por los slots ocupados

        if(_currentMemory <= 0)
        {
            _currentMemory = 0; // Asegura que la memoria no sea negativa
            _playerController.Respawn();
            ResetStatus(); // Si la memoria llega a cero, el jugador reaparece en el checkpoint y se restablece su salud y memoria al máximo
        }

    }

    private void DynamicSlotUpdate()
    {
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Resta el daño recibido a la salud del jugador
        if (currentHealth <= 0f)
        {
            currentHealth = 0f; // Asegura que la salud no sea negativa
            _playerController.Respawn(); // Si la salud llega a cero o menos, el jugador reaparece en el checkpoint
            ResetStatus(); // Restablece la salud y la memoria del jugador al reaparecer
        }
        else
        {
            if (!_playerController.isFlashingDamage)
            {
                StartCoroutine(_playerController.DamageFlash()); // Inicia la rutina de parpadeo de daño si el jugador recibe daño pero no muere
            }
        }
    }

    public void ResetStatus()
    {
        currentHealth = maxHealth; // Restablece la salud del jugador al máximo
        _currentMemory = _memoryPerSlot * _memorySlot; // Restablece la memoria del jugador al máximo permitido por los slots ocupados
    }


}
