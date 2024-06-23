using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Animator _animator;
	private bool isDead = false;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	public void Die()
	{
		if (!isDead)
		{
			isDead = true;
			Debug.Log("Enemy hit. Playing Death animation.");
			_animator.CrossFadeInFixedTime("Death", 0.1f); // Play the Death animation
			StartCoroutine(DestroyAfterAnimation());
		}
	}

	private IEnumerator DestroyAfterAnimation()
	{
		// Wait until the "Death" animation is playing
		while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			yield return null;
		}
		Debug.Log("Death animation started.");
		// Wait until the "Death" animation has finished
		while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			yield return null;
		}
		Debug.Log("Death animation finished.");
		Destroy(gameObject);
	}
}
