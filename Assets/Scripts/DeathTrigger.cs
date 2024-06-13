using System.Collections;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public bool IsDead { get; private set; }
    public LogicScript logic;
    [SerializeField]
    private float maxZPosition = -8f; // Set your threshold value for instant death

    private Vector3 initialPosition; // To reset to the initial position
    private Vector3 lastPosition; // To check if the player is stuck
    private float stuckCheckDuration = 3f; // Time to wait before checking if stuck
    private float stuckThreshold = 0.1f; // Distance threshold to consider the player stuck

    private void Start()
    {
        IsDead = false;
        initialPosition = transform.position; // Store the initial position
        lastPosition = transform.position; // Initialize last known position
        StartCoroutine(CheckIfStuck()); // Start checking if the player is stuck
    }

    private void Update()
    {
        if (transform.position.z < maxZPosition)
        {
            Die();
            logic.gameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
        {
            Die();
            logic.gameOver();
        }
    }

    public void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log("Player died!");
        Time.timeScale = 0; // Pause the game
        // Perform other actions such as playing death animation, reducing health, restarting level, etc.
    }

    private IEnumerator CheckIfStuck()
    {
        while (true)
        {
            yield return new WaitForSeconds(stuckCheckDuration);

            float distanceMoved = Vector3.Distance(lastPosition, transform.position);

            // Check if the player is stuck and not below the death threshold
            if (distanceMoved < stuckThreshold && transform.position.z >= maxZPosition)
            {
                ResetPosition();
            }

            lastPosition = transform.position;
        }
    }

    private void ResetPosition()
    {
        Debug.Log("Player is stuck. Resetting position.");
        Vector3 newPosition = transform.position; // Get the current position
        newPosition.z = initialPosition.z; // Reset only the z value
        transform.position = newPosition; // Set the new position
                                          // Optionally, reset other states or variables as needed
    }
}
