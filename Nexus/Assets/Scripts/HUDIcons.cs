using UnityEngine;
using UnityEngine.UI;

public class HUDIcons : MonoBehaviour
{
    public Image dashIcon; // Referencia al icono de dash en el HUD
    public Color readyColor = Color.white; // Color del icono cuando el dash está listo para usar
    public Color cooldownColor = Color.red; // Color del icono cuando el dash está en enfriamiento
    public float timeToNextDash; // Tiempo restante para el próximo dash disponible, que se actualizará en función del estado de enfriamiento del dash para mostrar al jugador cuánto tiempo falta antes de que el dash esté listo para usar nuevamente
    private PlayerDash playerDash; // Referencia al script PlayerDash para acceder a su estado de enfriamiento del dash
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dashIcon.color = readyColor; // Establece el color del icono de dash al color de listo al iniciar el juego para indicar que el dash está disponible para usar
        //dashIcon = GetComponent<Image>(); // Obtiene la referencia al componente Image del icono de dash para poder cambiar su color en función del estado del dash
        playerDash = Object.FindFirstObjectByType<PlayerDash>(); // Encuentra la instancia del script PlayerDash en la escena para acceder a su estado de enfriamiento del dash
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
