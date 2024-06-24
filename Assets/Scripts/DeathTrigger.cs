using System.Collections;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
	public bool IsDead { get; private set; }
	public LogicScript logic;
	[SerializeField]
	private float maxZPosition = -1f; // Set your threshold value for instant death

	private Vector3 initialPosition; // To reset to the initial position
	private Vector3 lastPosition; // To check if the player is stuck
	private float stuckCheckDuration = 3f; // Time to wait before checking if stuck
	private float stuckThreshold = 0.1f; // Distance threshold to consider the player stuck
	private Animator _animator;

	public AudioManager AudioManager;

	void Awake()
	{
		AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	private void Start()
	{
		logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
		IsDead = false;
		initialPosition = transform.position; // Store the initial position
		lastPosition = transform.position; // Initialize last known position
		StartCoroutine(CheckIfStuck()); // Start checking if the player is stuck
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (transform.position.z < maxZPosition)
		{
			Die();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("KillZone"))
		{
			Die();
		} 
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Die();
		}
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

	public void Die()
	{
		if (IsDead) return;

		IsDead = true;

		Debug.Log("Player died!");

		AudioManager.StopBackgroundMusic();
		AudioManager.PlaySFX(AudioManager.Death);

		// Play death animation
		_animator.CrossFadeInFixedTime("Death", 0.1f);

		// Disable further updates to prevent additional calls to Die()
		enabled = false;

		// Invoke the game over logic after a delay to allow the animation to play
		StartCoroutine(GameOverAfterAnimation());
	}

	private IEnumerator GameOverAfterAnimation()
	{
		// Wait until the "Death" animation is playing
		while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			yield return null;
		}

		// Wait until the "Death" animation has finished
		while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			yield return null;
		}

		Time.timeScale = 0f;
		// Trigger game over logic
		logic.gameOver();
	}

	private void ResetPosition()
	{
		Debug.Log("Player is stuck. Resetting position.");
		Vector3 newPosition = transform.position; // Get the current position
		newPosition.z = initialPosition.z; // Reset only the z value
		transform.position = newPosition; // Set the new position
	}

	public void ResetDeathState()
	{
		IsDead = false;
		enabled = true;
	}
}
