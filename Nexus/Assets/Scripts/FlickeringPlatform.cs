using System.Collections;
using UnityEngine;

public class FlickeringPlatform : MonoBehaviour
{
    [SerializeField] private float flickerDuration = 1f; // Duración total del parpadeo
    [SerializeField] private float flickerInterval = 0.1f; // Tiempo entre cada cambio de visibilidad
    [SerializeField] private float activeDuration = 2f; // Duración que la plataforma permanece visible y colisionable
    [SerializeField] private float inactiveDuration = 2f; // Duración que la plataforma permanece invisible y no colisionable
    //[SerializeField] private float elapsedTime = 0f; // Tiempo transcurrido desde el inicio del parpadeo
    private MeshRenderer meshRenderer; // Referencia al MeshRenderer del objeto
    private Collider platformCollider; // Referencia al Collider del objeto
    private Material platformMaterial; // Referencia al material del MeshRenderer para cambiar su color

    [Header("Colors")]
    public Color normalColor; // Color normal de la plataforma
    public Color warningColor = Color.red; // Color de advertencia durante el parpadeo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformMaterial = GetComponent<MeshRenderer>().material; // Obtiene la referencia al material del MeshRenderer
        normalColor = platformMaterial.color; // Guarda el color normal del material
        meshRenderer = GetComponent<MeshRenderer>(); // Obtiene la referencia al MeshRenderer del objeto
        platformCollider = GetComponent<Collider>(); // Obtiene la referencia al Collider del objeto
        StartCoroutine(FlickerCycle()); // Inicia la corrutina para el parpadeo de la plataforma
    }
    IEnumerator FlickerCycle()
    {
        while (true)
        {
            // Primera fase: La plataforma es visible y colisionable
            meshRenderer.enabled = true; // Asegura que el MeshRenderer esté habilitado
            platformCollider.enabled = true; // Asegura que el Collider esté habilitado
            platformMaterial.color = normalColor; // Establece el color normal del material
            yield return new WaitForSeconds(activeDuration); // Espera la duración del parpadeo antes de iniciar el siguiente ciclo

            // Comienza el parpadeo - 2DO Estado: Parpadeo
            float elapsedTime = 0f; //Siempre se reinicia el tiempo transcurrido al iniciar el parpadeo
            while (elapsedTime < flickerDuration) // Mientras el tiempo transcurrido sea menor que la duración del parpadeo
            {
                if(platformMaterial.color == normalColor)
                {
                    platformMaterial.color = warningColor; // Cambia al color de advertencia
                }
                else
                {
                    platformMaterial.color = normalColor; // Cambia al color normal
                }
                //meshRenderer.enabled = !meshRenderer.enabled; // Alterna la visibilidad del MeshRenderer
                //platformCollider.enabled = meshRenderer.enabled; // Sincroniza el estado del Collider con el MeshRenderer
                elapsedTime += flickerInterval; // Incrementa el tiempo transcurrido en cada iteración del bucle
                yield return new WaitForSeconds(flickerInterval); // Espera el intervalo de tiempo antes de cambiar nuevamente la visibilidad
            }
            // Tercer estado: Inactivo
            // Asegura que la plataforma quede invisible y no colisionable al finalizar el parpadeo
            meshRenderer.enabled = false;
            platformCollider.enabled = false;
            platformMaterial.color = normalColor;
            // Espera el tiempo de inactividad antes de reactivar la plataforma
            yield return new WaitForSeconds(inactiveDuration); // Espera el tiempo de inactividad antes de reactivar la plataforma
        }
    }
}
