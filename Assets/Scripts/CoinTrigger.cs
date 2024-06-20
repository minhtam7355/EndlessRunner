using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Notify the LogicScript that a coin has been collected
            LogicScript logicScript = FindObjectOfType<LogicScript>();
            if (logicScript != null)
            {
                logicScript.CollectCoin();
            }

            // Disable the coin object (make it disappear)
            gameObject.SetActive(false);

            // Alternatively, you can return the coin to the pool if using pooling
            // Example: CoinPool.Instance.ReturnCoin(gameObject);
        }
    }

    void Update()
    {
        // Rotate the coin smoothly around the Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
