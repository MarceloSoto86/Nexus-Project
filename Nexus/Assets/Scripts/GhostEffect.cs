using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    
    public float ghostAppearTime = 0.5f; // Tiempo que tarda en aparecer el fantasma
    public float ghostRate = 0.04f; // Frecuencia con la que se generan los fantasmas (en segundos)
    public GameObject ghostPrefab; // Prefab del objeto fantasma
    public SkinnedMeshRenderer _ghostSkinnedMeshRenderer; // Referencia al SkinnedMeshRenderer del jugador para copiar su apariencia


    private bool _isTrailActive = false; // Indica si el efecto de fantasma está activo

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
            Mesh frozenMesh = new Mesh();
            // Esto toma los huesos deformados por el "a_Dash" en este frame y los fusiona en una malla estática
            _ghostSkinnedMeshRenderer.BakeMesh(frozenMesh);
            // Generamos el fantasma
            GameObject currentGhost = Instantiate(ghostPrefab, _ghostSkinnedMeshRenderer.transform.position, _ghostSkinnedMeshRenderer.transform.rotation);
            // REFUERZO POR CÓDIGO: Nos aseguramos de que no tenga collider al nacer
            if (currentGhost.TryGetComponent<MeshFilter>(out MeshFilter ghostFilter))
            {
                ghostFilter.mesh = frozenMesh;
            }
            if (currentGhost.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }
            Destroy(currentGhost, ghostAppearTime); // Destruye el fantasma después de que termine su animación (ajusta el tiempo según la duración de tu animación de fantasma)
            StartCoroutine(CleanupGhostMesh(frozenMesh, ghostAppearTime));
            yield return new WaitForSeconds(ghostRate); // Wait before creating the next ghost
        }

    }
    private IEnumerator CleanupGhostMesh(Mesh meshToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (meshToDestroy != null)
        {
            // Esto elimina físicamente los datos binarios de los vértices de la memoria del motor
            Destroy(meshToDestroy);
        }
    }
}
