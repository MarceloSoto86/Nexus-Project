using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    private Transform platMove;
    private Transform currentTarget;
    [SerializeField] private float speed = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platMove = transform.GetChild(2);
        posA = transform.GetChild(0);
        posB = transform.GetChild(1);
        currentTarget = posA;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        float distance = Vector3.Distance(platMove.position, currentTarget.position);
        Debug.Log("Distancia: " + distance);
        
        if (distance < 0.1f)
        {
            currentTarget = currentTarget == posA ? posB : posA; // Cambia el objetivo entre posA y posB
        }

        Vector3 direction = (currentTarget.position - platMove.position).normalized; // Calcula la dirección hacia el objetivo
        platMove.transform.Translate(direction * speed * Time.deltaTime); // Mueve la plataforma hacia el objetivo
    }
}
