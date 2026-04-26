using System.Collections;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    
    public float ghostAppearTime = 0.5f; // Tiempo que tarda en aparecer el fantasma
    public float ghostRate = 0.04f; // Frecuencia con la que se generan los fantasmas (en segundos)
    public GameObject ghostPrefab; // Prefab del objeto fantasma
    

    private bool _isTrailActive = false; // Indica si el efecto de fantasma está activo



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void StartTrail()
    {
        
        StartCoroutine(GhostTrailGeneration());
    }

    public void StopTrail()
    {
        _isTrailActive = false; 
       // StopCoroutine(GhostTrailGeneration());
    }

    IEnumerator GhostTrailGeneration ( )
    {
        _isTrailActive = true; // Activa el efecto de fantasma
        while (_isTrailActive)
        {
            // Create a ghost object at the current position and rotation
           Instantiate(ghostPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(ghostRate); // Wait before creating the next ghost
        }
    }
}
