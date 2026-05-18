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
        StopAllCoroutines();
    }

    IEnumerator GhostTrailGeneration ( )
    {
        _isTrailActive = true; // Activa el efecto de fantasma
        while (_isTrailActive)
        {
          // Generamos el fantasma
        GameObject currentGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        // REFUERZO POR CÓDIGO: Nos aseguramos de que no tenga collider al nacer
        if (currentGhost.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false; 
        }
            yield return new WaitForSeconds(ghostRate); // Wait before creating the next ghost
        }
    }
}
