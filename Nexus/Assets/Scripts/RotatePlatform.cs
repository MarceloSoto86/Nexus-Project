using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0, Space.Self);
    }
}
