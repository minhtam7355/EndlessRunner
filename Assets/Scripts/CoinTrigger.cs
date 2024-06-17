using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTrigger : MonoBehaviour
{
    [SerializeField]
    public Text coinText;
    private static int coinCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Increment the coin count
            coinCount++;

            // Update the coin text UI element
            UpdateCoinText();

            // Disable the coin object (make it disappear)
            gameObject.SetActive(false);

            // Alternatively, you can return the coin to the pool if using pooling
            // Example: CoinPool.Instance.ReturnCoin(gameObject);
        }
    }
    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }
}
