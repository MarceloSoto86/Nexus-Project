using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    public CanvasGroup pauseMenuCanvasGroup; // Referencia al CanvasGroup del menú de pausa para controlar su visibilidad y la interactividad de sus elementos
    public GameObject pauseMenu; // Referencia al Canvas del menú de pausa

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false; // Oculta el cursor del mouse al iniciar el juego para proporcionar una experiencia de juego más inmersiva, especialmente en juegos de primera persona o juegos que requieren un control preciso del mouse para la navegación y la interacción con el entorno del juego
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor del mouse al iniciar el juego para que esté centrado en la pantalla y no se mueva libremente, lo que es común en juegos de primera persona o juegos que requieren un control preciso del mouse para la navegación y la interacción con el entorno del juego
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Verifica si se presiona la tecla Escape para alternar el estado de pausa del juego
        {
            TogglePause(); // Llama al método TogglePause para cambiar el estado de pausa del juego y mostrar u ocultar el menú de pausa en consecuencia
        }

    }

    public void TogglePause()
    {
        IsPaused = !IsPaused; // Cambia el estado de pausa del juego alternando el valor de IsPaused entre true y false
        if (IsPaused)
        {
            Time.timeScale = 0f; // Detiene el tiempo del juego estableciendo timeScale a 0, lo que hace que todas las actualizaciones del juego se detengan, incluyendo la física, animaciones y otras mecánicas del juego, para crear un efecto de pausa en el juego
            pauseMenu.SetActive(true); // Activa el Canvas del menú de pausa para mostrarlo al jugador cuando el juego está pausado
            if (pauseMenuCanvasGroup != null)
            {
                pauseMenuCanvasGroup.alpha = 1f; // Establece la opacidad del menú de pausa a 1 para hacerlo visible al jugador cuando el juego está pausado
                pauseMenuCanvasGroup.interactable = true; // Permite la interacción con los elementos del menú de pausa al establecer interactable a true, lo que permite al jugador navegar por el menú y seleccionar opciones mientras el juego está pausado
                pauseMenuCanvasGroup.blocksRaycasts = true; // Permite que el menú de pausa bloquee los raycasts al establecer blocksRaycasts a true, lo que evita que los clics del mouse o las interacciones táctiles pasen a través del menú de pausa y afecten a otros elementos del juego mientras el menú está activo
                Cursor.visible = true; // Hace visible el cursor del mouse para que el jugador pueda interactuar con el menú de pausa utilizando el mouse mientras el juego está pausado
                Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor del mouse para que el jugador pueda moverlo libremente y hacer clic en los elementos del menú de pausa mientras el juego está pausado, lo que permite una experiencia de usuario más intuitiva al interactuar con el menú de pausa.
            }
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo del juego estableciendo timeScale a 1, lo que permite que todas las actualizaciones del juego se reanuden normalmente después de haber estado pausado
            pauseMenu.SetActive(false); // Desactiva el Canvas del menú de pausa para ocultarlo al jugador cuando el juego se reanuda
            if (pauseMenuCanvasGroup != null)
            {
                pauseMenuCanvasGroup.alpha = 0f; // Establece la opacidad del menú de pausa a 0 para ocultarlo visualmente al jugador cuando el juego se reanuda
                pauseMenuCanvasGroup.interactable = false; // Desactiva la interacción con los elementos del menú de pausa al establecer interactable a false, lo que evita que el jugador pueda interactuar con el menú de pausa mientras el juego está activo nuevamente
                pauseMenuCanvasGroup.blocksRaycasts = false; // Permite que los raycasts pasen a través del área donde estaba el menú de pausa al establecer blocksRaycasts a false, lo que permite que las interacciones normales del juego ocurran sin interferencia del menú de pausa después de reanudar el juego
                Cursor.visible = false; // Oculta el cursor del mouse para que no sea visible al jugador mientras el juego está activo nuevamente después de haber estado pausado, lo que proporciona una experiencia de juego más inmersiva al eliminar distracciones visuales innecesarias
                Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor del mouse para que esté centrado en la pantalla y no se mueva libremente después de reanudar el juego, lo que es común en juegos de primera persona o juegos que requieren un control preciso del mouse para la navegación y la interacción con el entorno del juego.
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit(); // Cierra la aplicación del juego cuando se llama a este método, lo que es útil para proporcionar una opción de salida al jugador desde el menú de pausa o cualquier otro lugar del juego donde se desee permitir que el jugador salga del juego.
        //Debug.log("Quit Game"); // Imprime un mensaje en la consola para indicar que se ha llamado al método QuitGame, lo que puede ser útil para depuración o para confirmar que la función de salida del juego se ha activado correctamente.
    }
}
