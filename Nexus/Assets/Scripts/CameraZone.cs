using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [SerializeField] Vector3 offsetNode;
    [SerializeField] Vector3 rotationNode;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.ChangeView(offsetNode, rotationNode);
            }
            Debug.Log("Cámara cambiada!");
        }
    }

}
