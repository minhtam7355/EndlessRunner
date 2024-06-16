using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
	private Animator _animator;
	public bool IsDead { get; private set; }
	public LogicScript logic;
	[SerializeField]
	private float maxZPosition; // Set your threshold value

	void Start()
	{
		IsDead = false;
		_animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
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

	public void Die()
	{
		if (IsDead) return;

		IsDead = true;

		Debug.Log("Player died!");

		// Play death animation
		_animator.Play("Death");

		// Disable further updates to prevent additional calls to Die()
		enabled = false;

		// Invoke the game over logic after a delay to allow the animation to play
		StartCoroutine(GameOverAfterAnimation());
	}

	IEnumerator GameOverAfterAnimation()
	{
		// Wait for the duration of the death animation
		yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
		Time.timeScale = 0f;
		// Trigger game over logic
		logic.gameOver();
	}
}
