using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public string filePath;
    public static SaveSystem instance; 
     private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el objeto se destruya al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Destruye el objeto si ya existe una instancia
        }
    }

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "nexus_save.json");
       
    }

    // Podés poner esto temporalmente en el Update de tu SaveSystem para limpiar el disco
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete)) // Si presionás la tecla Borrar (Retroceso)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log(" ¡Archivo de guardado eliminado con éxito! Reiniciá el juego.");
            }
        }
    }


    public void SaveGame(PlayerStatus player)
    {
        PlayerData data = new PlayerData();
        // Aquí deberías asignar los valores de tu juego a las variables de data
        data.memoryCurrentSlotsUnlocked = player._memorySlot;
        data.currentHealth = player.currentHealth;
        data.xPosition = player.transform.position.x;
        data.yPosition = player.transform.position.y;
        data.zPosition = player.transform.position.z;
        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(filePath, json);
        Debug.Log("Juego guardado en: " + filePath);
    }

   /* public void SaveGame()
    {
        if (File.Exists(filePath))
        {
            Debug.LogWarning("No se encontró el archivo de guardado para guardar el juego en: " + filePath);
            return;
        }
        PlayerData data = new PlayerData();
        // Aquí deberías asignar los valores de tu juego a las variables de data
        data.memoryCurrentSlotsUnlocked = 1; // Asigna un valor predeterminado o el valor actual del juego
        data.currentHealth = 100; // Asigna un valor predeterminado o el valor actual del juego
        data.xPosition = 91f; // Asigna un valor predeterminado o el valor actual del juego
        data.yPosition = 0.2f; // Asigna un valor predeterminado o el valor actual del juego
        data.zPosition = -30f; // Asigna un valor predeterminado o el valor actual del juego
        
        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(filePath, json);
        Debug.Log("Juego guardado en: " + filePath);
    }*/

    public PlayerData LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            // Aquí deberías asignar los valores de data a tu juego
            Debug.Log("Juego cargado desde: " + filePath);
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado en: " + filePath);
        }
        return null;
    }

    public void DeleteSavedData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Archivo de guardado eliminado con éxito.");
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado para eliminar en: " + filePath);
        }
    }
}
