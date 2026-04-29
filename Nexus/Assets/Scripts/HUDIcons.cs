using UnityEngine;
using UnityEngine.UI;

public class HUDIcons : MonoBehaviour
{
    public Image dashIcon; // Referencia al icono de dash en el HUD
    public Color readyColor = Color.white; // Color del icono cuando el dash está listo para usar
    public Color cooldownColor = Color.red; // Color del icono cuando el dash está en enfriamiento
    public float timeToNextDash; // Tiempo restante para el próximo dash disponible, que se actualizará en función del estado de enfriamiento del dash para mostrar al jugador cuánto tiempo falta antes de que el dash esté listo para usar nuevamente
    public Slider _healthIndicatorSlider; // Referencia al slider de salud en el HUD para mostrar la salud actual del jugador, lo que podría ser útil para mostrar un icono de salud o una barra de salud en el futuro
    public Slider _memoryIndicatorSlider; // Referencia al slider de memoria en el HUD para mostrar la memoria actual del jugador, lo que podría ser útil para mostrar un icono de memoria o una barra de memoria en el futuro
    private PlayerDash playerDash; // Referencia al script PlayerDash para acceder a su estado de enfriamiento del dash
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerStatus _playerStatus; // Referencia al script PlayerStatus para acceder a su estado de salud y memoria del jugador, lo que podría ser útil para mostrar otros iconos relacionados con la salud o la memoria en el HUD en el futuro
    

    void Start()
    {
        dashIcon.color = readyColor; // Establece el color del icono de dash al color de listo al iniciar el juego para indicar que el dash está disponible para usar
        //dashIcon = GetComponent<Image>(); // Obtiene la referencia al componente Image del icono de dash para poder cambiar su color en función del estado del dash
        playerDash = Object.FindFirstObjectByType<PlayerDash>(); // Encuentra la instancia del script PlayerDash en la escena para acceder a su estado de enfriamiento del dash
        _playerStatus = PlayerController.player.GetComponent<PlayerStatus>(); // Obtiene la referencia al script PlayerStatus del jugador para acceder a su estado de salud y memoria, lo que podría ser útil para mostrar otros iconos relacionados con la salud o la memoria en el HUD en el futuro

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStatus == null)
        {
            if (PlayerController.player != null)
            {
                _playerStatus = PlayerController.player.GetComponent<PlayerStatus>(); // Si la referencia al script PlayerStatus es nula, intenta obtenerla nuevamente del jugador para asegurarse de tener acceso a su estado de salud y memoria, lo que podría ser útil para mostrar otros iconos relacionados con la salud o la memoria en el HUD en el futuro
            }
            return; // Si la referencia al script PlayerStatus sigue siendo nula después de intentar obtenerla, salir del método Update para evitar errores al intentar acceder a sus propiedades
        }

        timeToNextDash = playerDash.nextDashTime - Time.time; // Calcula el tiempo restante para el próximo dash disponible restando el tiempo actual del tiempo para el próximo dash disponible, lo que permite mostrar al jugador cuánto tiempo falta antes de que el dash esté listo para usar nuevamente

        timeToNextDash = Mathf.Clamp(timeToNextDash, 0f, playerDash.dashCooldown); // Limita el tiempo restante para el próximo dash entre 0 y el tiempo de enfriamiento del dash para evitar que se muestre un tiempo mayor al tiempo de enfriamiento, lo que garantiza que la información mostrada al jugador sea precisa y relevante en función del estado actual del dash

        dashIcon.fillAmount = 1f - (timeToNextDash / playerDash.dashCooldown); // Actualiza la cantidad de relleno del icono de dash en función del tiempo restante para el próximo dash disponible, lo que proporciona una representación visual del estado de enfriamiento del dash al jugador, donde el icono se llenará gradualmente a medida que el tiempo restante disminuya hasta que esté completamente lleno cuando el dash esté listo para usar nuevamente

        if (dashIcon != null)
        {
            if (timeToNextDash > 0f)
            {
                dashIcon.color = cooldownColor; // Cambia el color del icono de dash al color de enfriamiento si el tiempo restante para el próximo dash es mayor que cero, lo que indica visualmente al jugador que el dash está en enfriamiento y no está disponible para usar
            }
            else
            {
                dashIcon.color = readyColor; // Cambia el color del icono de dash al color de listo si el tiempo restante para el próximo dash es cero o menor, lo que indica visualmente al jugador que el dash está listo para usar nuevamente
            }
        }

        _healthIndicatorSlider.value = _playerStatus.currentHealth / _playerStatus.maxHealth; // Actualiza el valor del slider de salud en el HUD en función de la salud actual del jugador dividida por la salud máxima para mostrar al jugador su estado de salud actual de manera visual a través del slider, lo que podría ser útil para mostrar un icono de salud o una barra de salud en el futuro
        _memoryIndicatorSlider.value = _playerStatus._currentMemory / (_playerStatus._memoryPerSlot * _playerStatus._memorySlot); // Actualiza el valor del slider de memoria en el HUD en función de la memoria actual del jugador dividida por la memoria máxima para mostrar al jugador su estado de memoria actual de manera visual a través del slider, lo que podría ser útil para mostrar un icono de memoria o una barra de memoria en el futuro

    }
}
