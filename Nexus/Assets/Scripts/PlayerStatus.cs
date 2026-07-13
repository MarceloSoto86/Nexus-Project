using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int _memoryMax = 100; // Valor máximo de memoria del jugador
    public int _memorySlot = 1; // Número de slots de memoria ocupados (puede ser un valor entre 0 y 10, por ejemplo)
    public int _currentSlot; // Número de slots de memoria actualmente ocupados (puede ser un valor entre 0 y _memorySlot)
    public int _memoryPerSlot = 25; // Cantidad de memoria que cada slot puede contener
    public float _currentMemory = 25f; // Valor actual de memoria del jugador
    public float _memoryDecreaseRate; // Tasa a la que la memoria disminuye
    public float currentHealth; // Valor actual de salud del jugador
    public float maxHealth = 100; // Valor máximo de salud del jugador 

    public bool isDashUnlocked;

    public List<string> activeCollectedItemIDs = new List<string>();

    public PlayerEvents _playerEvents;
    public PlayerController _playerController; // Referencia al script PlayerController para acceder a su estado y funciones

    public static event Action OnMemoryStoreUnlocked; // Evento que se dispara cuando se desbloquea un nuevo slot de memoria

    private void Start()
    {
        _playerEvents = GetComponent<PlayerEvents>();
        _playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        //_currentMemory = _memoryMax;
        _currentMemory = 25f; // Inicialmente, la memoria actual se establece en función de los slots ocupados
        _memorySlot = 1; // Inicialmente, el jugador tiene un slot de memoria ocupado
        PlayerData savedData = SaveSystem.instance.LoadGame(); // Carga los datos guardados del jugador al iniciar el juego
        if (savedData != null)
        {
            activeCollectedItemIDs = savedData.collectedItemIDs;
            _memorySlot = savedData.memoryCurrentSlotsUnlocked;
            _currentMemory = _memorySlot * _memoryPerSlot; // Establece la memoria actual en función de los slots ocupados al cargar los datos guardados del jugador
            currentHealth = savedData.currentHealth;
            isDashUnlocked = savedData.dashUnlocked;

            Vector3 loadedPosition = new Vector3(savedData.xPosition, savedData.yPosition, savedData.zPosition);

            Rigidbody playerRb = GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                {
                    playerRb.linearVelocity = Vector3.zero; // Detiene cualquier movimiento residual del jugador al cargar su posición
                    playerRb.angularVelocity = Vector3.zero; // Detiene cualquier rotación residual del jugador al cargar su posición
                }
            }
            transform.position = loadedPosition; // Establece la posición del jugador a la posición guardada en los datos cargados para que el jugador comience en el mismo lugar donde guardó su progreso
            Physics.SyncTransforms(); // Sincroniza las transformaciones físicas para asegurarse de que la posición del jugador se actualice correctamente en el motor de física después de cargar los datos
            if (_playerController != null)
             {
                  _playerController.SetCheckpoint(loadedPosition); // Establece el checkpoint del jugador a la posición cargada para que el jugador reaparezca en el mismo lugar donde guardó su progreso al morir o reiniciar el juego
                _playerController.isDashUnlocked = savedData.dashUnlocked;

                if (_playerController.dashHUDIcon != null)
                {
                    _playerController.dashHUDIcon.enabled = _playerController.isDashUnlocked;
                }

                //Debug.log($"<color=green>[SAVE SYSTEM]: Dash restaurado desde archivo -> {_playerController.isDashUnlocked}</color>");
            }
            _memorySlot = savedData.memoryCurrentSlotsUnlocked;
            _currentMemory = _memorySlot * _memoryPerSlot; // Si carga 1 slot, arranca en 25 de energía, no en 100. 
            _playerEvents.RaiseSanityChanged(_currentMemory, _memorySlot); // Actualiza el HUD de memoria con los datos cargados del jugador para reflejar su estado de memoria actual al iniciar el juego        
        }
    }

    public void Update()
    {
        _currentMemory -= (_memoryDecreaseRate * Time.deltaTime); // Disminuye la memoria actual según la tasa de disminución
        _currentMemory = Mathf.Clamp(_currentMemory, 0, _memoryPerSlot * _memorySlot); // Asegura que la memoria actual no exceda el máximo permitido por los slots ocupados
        if (_playerEvents != null)
        {
            _playerEvents.RaiseSanityChanged(_currentMemory, _memorySlot);
        }
        if (_currentMemory <= 0)
        {
            _currentMemory = 0; // Asegura que la memoria no sea negativa       
            _playerController.SwitchState(_playerController.dyingFromInsanityState); // Cambia al estado de muerte por locura si la memoria llega a cero
        }
    }
    public void AddCollectedItemID(string itemID)
    {
        if (!activeCollectedItemIDs.Contains(itemID))
        {
            activeCollectedItemIDs.Add(itemID);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Resta el daño recibido a la salud del jugador
        if (currentHealth <= 0f)
        {
            currentHealth = 0f; // Asegura que la salud no sea negativa
                                //_playerController.Respawn(); // Si la salud llega a cero o menos, el jugador reaparece en el checkpoint
            _playerController.SwitchState(_playerController.dyingFromDamageState); // Cambia al estado de muerte por daño para reproducir la animación de muerte y luego reaparecer al jugador
        }
        else
        {
            if (!_playerController.isFlashingDamage)
            {
                StartCoroutine(_playerController.DamageFlash()); // Inicia la rutina de parpadeo de daño si el jugador recibe daño pero no muere
            }
            _playerController.SwitchState(_playerController.isFlashingDamageState);
        }
    }
    public void ResetStatus()
    {
        currentHealth = maxHealth; // Restablece la salud del jugador al máximo
        _currentMemory = _memoryPerSlot * _memorySlot; // Restablece la memoria del jugador al máximo permitido por los slots ocupados
    }
    public void UnlockNextMemorySlot()
    {
        if (_memorySlot < 4)
        {
            _memorySlot++;
            RestoreMemory(_memoryPerSlot);

            // Invocación a través de la arquitectura de eventos limpia
            if (_playerEvents != null)
            {
                PlayerEvents.RaiseMemoryStoreUnlocked(); 
            }
        }
    }
    public void RestoreMemory(float amount)
    {
        _currentMemory = Mathf.Min(_currentMemory + amount, _memoryPerSlot * _memorySlot); // Restaura la memoria del jugador sin exceder el máximo permitido por los slots ocupados
    }
}
