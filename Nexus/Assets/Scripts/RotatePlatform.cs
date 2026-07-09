using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    //[SerializeField] private float rotationSpeed = 10f;
    public Vector3 rotationSpeedV3;
    //public Rigidbody platformRb;

    //[SerializeField] private Transform visualTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*if (platformRb == null)
        {
            platformRb = GetComponent<Rigidbody>();
            platformRb.interpolation = RigidbodyInterpolation.None;
        }*/
    }

    private void FixedUpdate()
    {
        /*//
        if (platformRb != null)
        {
            Vector3 thisFrameDegrees = rotationSpeedV3 * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(thisFrameDegrees);
            platformRb.MoveRotation(platformRb.rotation * deltaRotation);
        }
*/
    }
    private void Update()
    {
        transform.Rotate(rotationSpeedV3 * Time.deltaTime, Space.Self);
        /*if(visualTransform != null && platformRb != null)
        { 
            visualTransform.rotation = platformRb.rotation;
        }*/
    }
}
