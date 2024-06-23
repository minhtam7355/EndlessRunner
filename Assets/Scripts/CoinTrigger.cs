using UnityEngine;
using UnityEngine.UI;

public class CoinTrigger : MonoBehaviour
{
	public AudioManager AudioManager;
	public LogicScript logicScript; // Reference to the LogicScript component
	public float rotationSpeed = 100.0f;

	private void Awake()
	{
		AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		logicScript = FindObjectOfType<LogicScript>(); // Find LogicScript component in the scene
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// Call CollectCoin method from LogicScript to increment session coins
			logicScript.CollectCoin();

			// Play coin collect sound
			AudioManager.PlaySFX(AudioManager.CoinCollect);

			// Disable the coin object (make it disappear)
			gameObject.SetActive(false);
		}
	}

	void Update()
	{
		// Rotate the coin smoothly around the Y axis
		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
	}
}
