using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    //[SerializeField] private float rotationSpeed = 10f;
    public Vector3 rotationSpeedV3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeedV3 * Time.deltaTime,Space.Self);
    }
}
