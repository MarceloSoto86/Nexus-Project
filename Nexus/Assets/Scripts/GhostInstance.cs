using UnityEngine;

public class GhostInstance : MonoBehaviour
{
    public MeshRenderer _meshRenderer; // Referencia al MeshRenderer del objeto
    public Material _ghostMaterial;  // Material para el efecto fantasma
    public float _fadeSpeed = 1.0f; // Velocidad de desvanecimiento
    public Color _color;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if(_meshRenderer != null)
        {
            _color = _meshRenderer.material.color; // Almacena el color original del material para modificar su transparencia durante el desvanecimiento
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (_meshRenderer == null)
        {
            Debug.LogWarning("MeshRenderer no asignado en GhostEffect.");
            return;
        }
        else
        {

            // Create a ghost effect by rendering the object with the ghost material
            _color.a -= _fadeSpeed * Time.deltaTime;
            _meshRenderer.material.color = _color; // Update the color of the material to create the fading effect
            if (_color.a <= 0)
            {
                Destroy(gameObject); // Destroy the object when it becomes fully transparent
            }


        }
    }
}
