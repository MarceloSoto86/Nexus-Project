using UnityEngine;

public class PlatformPathMover : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints for the platform to follow
    public float speed = 2f; // Speed at which the platform moves
    private int currentWaypointIndex = 0; // Index of the current waypoint
    //private int currentWaypoint = 0;
    [SerializeField] private float timer = 1f; // Time to wait at each waypoint
    [SerializeField] private float waitTime = 1f; // Time to wait at each waypoint
    private bool isWaiting = false; // Flag to indicate if the platform is currently waiting at a waypoint

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       /* waypoints = new Transform[transform.childCount]; // Initialize the waypoints array based on the number of child objects
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i); // Assign each child transform to the waypoints array
            waypoints[i].SetParent(null);
        }*/
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        MovePlatform(); // Call the method to move the platform towards the current waypoint
    }

    // This method moves the platform towards the current waypoint
    private void MovePlatform()
    {
        if (isWaiting)
        {
            timer -= Time.deltaTime; // Decrease the waiting time
            if (timer <= 0f) // If the waiting time has elapsed
            {
                isWaiting = false; // Stop waiting
            }
            return; // Exit the method while waiting
        }

        if (waypoints.Length == 0) return; // If there are no waypoints, exit the method
        Transform targetWaypoint = waypoints[currentWaypointIndex]; // Get the current target waypoint

        //Vector3 direction = (targetWaypoint.position - transform.position).normalized; // Calculate the direction towards the target waypoint
        //transform.Translate(direction * speed * Time.deltaTime, Space.World); // Move the platform towards the target waypoint

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime); // Move the platform towards the target waypoint at a constant speed
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position); // Calculate the distance to the target waypoint
        if (distanceToWaypoint < 0.1f) // If the platform is close enough to the waypoint
        {
            isWaiting = true; // Start waiting at the waypoint
            timer = waitTime; // Reset the waiting time
            UpdateIndex(); // Move to the next waypoint
        }
    }

    // This method updates the current waypoint index to the next waypoint in the array
    private void UpdateIndex()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Increment the waypoint index and wrap around if it exceeds the array length
    }
}
