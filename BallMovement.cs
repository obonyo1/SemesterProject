using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public int priority; // Priority of the ball (lower value = higher priority)
    public float speed = 2f; // Speed of the ball
    public float maxAllowedTime = 5f; // Maximum time the ball can move before being destroyed
    private Vector3 goalPosition; // Target position for the ball
    public bool isMoving = false; // Whether the ball is currently moving
    private float timeElapsed = 0f; // Time the ball has been moving

    void Start()
    {
        // Set the goal position at x=10
        goalPosition = new Vector3(10, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (isMoving)
        {
            MoveBall(); // Call the movement logic every frame
            CheckTimeLimit(); // Check if the ball has exceeded its allocated time
        }
    }

    public void StartMoving()
    {
        isMoving = true; // Begin movement
        timeElapsed = 0f; // Reset the timer
    }

    public void StopMoving()
    {
        isMoving = false; // Stop the ball from moving
    }

    private void MoveBall()
    {
        // Increment the elapsed time
        timeElapsed += Time.deltaTime;

        // Move the ball towards the goal position
        transform.position = Vector3.MoveTowards(transform.position, goalPosition, speed * Time.deltaTime);

        // Check if the ball has reached the goal position on the x-axis
        if (Mathf.Abs(transform.position.x - goalPosition.x) < 0.1f)
        {
            Debug.Log($"{gameObject.name} reached the goal at x=10.");
            isMoving = false; // Stop the ball from moving
            Destroy(gameObject); // Remove the ball from the scene
        }
    }

    private void CheckTimeLimit()
    {
        // Destroy the ball if it exceeds its allocated time
        if (timeElapsed > maxAllowedTime)
        {
            Debug.Log($"{gameObject.name} exceeded its allocated time and was removed.");
            isMoving = false; // Stop the ball from moving
            Destroy(gameObject);
        }
    }
}
