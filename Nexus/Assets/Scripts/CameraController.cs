using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float xAxis = 0f;
    [SerializeField] private float yAxis = 0f;
    [SerializeField] private float zAxis = 0f;
    [SerializeField] Transform _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.position + new Vector3(xAxis, yAxis, zAxis);
        transform.LookAt(_player);
    }
}
