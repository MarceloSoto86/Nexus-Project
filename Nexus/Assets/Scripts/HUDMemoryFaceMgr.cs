using UnityEngine;
using UnityEngine.UI;

public class HUDMemoryFaceMgr : MonoBehaviour
{
    [Header("HUD UI Components")]
    public Image currentHUDFace;
    //public Sprite[] faceSprites; // Array de sprites para las diferentes caras
    public ScriptableObject[] faceDataSO; // Array de ScriptableObjects para los datos de las caras
    private MemoryFlashes[] preCastedFaceData;
    public int faceCount = 0;
    //[Header("Event Listing")]
    //[SerializeField] private PlayerEvents memoryUnlockedChannel;

    private void Start()
    {
        preCastedFaceData = new MemoryFlashes[faceDataSO.Length];
        for (int i = 0; i < faceDataSO.Length; i++)
        {
            if (faceDataSO[i] is MemoryFlashes castedData)
            {
                preCastedFaceData[i] = castedData;
            }
            else
            {
                Debug.LogError($"¡Ojo! El elemento {i} en FaceDataSO del HUD no es un SO de tipo 'FaceData'.");
            }
        }
        if (preCastedFaceData != null && preCastedFaceData.Length > 0 && currentHUDFace != null)
        {
            currentHUDFace.sprite = preCastedFaceData[0]._nitidFaceImg;

            // Dejamos faceCount en 0. Así, cuando el jugador recolecte el PRIMER suero 
            // en el nivel y se dispare 'UpdateHUDMemoryFace', leerá el índice 0 o avanzará 
            // según cómo manejes la actualización de los datos del Supervisor.
        }
    }


    private void OnEnable()
    {

        PlayerEvents.OnMemoryStoreUnlocked += UpdateHUDMemoryFace; // Suscribirse al evento de cambio de cara de memoria del jugador
    }

    private void OnDisable()
    {
        PlayerEvents.OnMemoryStoreUnlocked -= UpdateHUDMemoryFace; // Cancelar la suscripción al evento al desactivar el HUD
    }

    private void UpdateHUDMemoryFace()
    {
        // Comprobar la longitud del array pre-casteado
        if (preCastedFaceData != null && faceCount < preCastedFaceData.Length)
        {
            //  Lectura directa, súper eficiente
            currentHUDFace.sprite = preCastedFaceData[faceCount]._nitidFaceImg;
            faceCount++;
        }
        else if (preCastedFaceData == null || preCastedFaceData.Length == 0)
        {
            Debug.LogError("El array FaceData del HUD está vacío o no se ha inicializado correctamente.");
        }
        else
        {
            Debug.LogWarning("No hay más caras de memoria para mostrar en el HUD.");
        }

    }
}
