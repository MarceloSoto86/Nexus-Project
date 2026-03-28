using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    private Transform platMove;
    private Transform currentTarget;
    [SerializeField] private float speed = 4f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created and is used to initialize the platform's movement and player locking variables
    void Start()
    {
        platMove = transform.GetChild(2);
        posA = transform.GetChild(0);
        posB = transform.GetChild(1);
        currentTarget = posA;


    }

    // Update is called once per frame and is used to implement the main behavior of the platform, including movement and player locking
    void Update()
    {
        MovePlatform();
       
    }
    // Esta función mueve la plataforma entre las posiciones posA y posB
    private void MovePlatform()
    {
        float distance = Vector3.Distance(platMove.position, currentTarget.position);
        //Debug.Log("Distancia: " + distance);
        
        if (distance < 0.1f)
        {
            currentTarget = currentTarget == posA ? posB : posA; // Cambia el objetivo entre posA y posB
        }

        Vector3 direction = (currentTarget.position - platMove.position).normalized; // Calcula la dirección hacia el objetivo
        platMove.transform.Translate(direction * speed * Time.deltaTime, Space.World); // Mueve la plataforma hacia el objetivo
    }

    
    
}
