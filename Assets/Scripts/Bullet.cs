using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float life = 3;
	public int pointsForEnemy = 100; // Points awarded for destroying an enemy
	public int coinsForEnemy = 3; // Coins awarded for destroying an enemy

	void Awake()
	{
		Destroy(gameObject, life);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			// Play the death animation on the enemy
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.Die();
			}

			// Call the RewardForEnemyDestruction method on the LogicScript
			LogicScript logic = FindObjectOfType<LogicScript>();
			if (logic != null)
			{
				logic.RewardForEnemyDestruction(pointsForEnemy, coinsForEnemy);
			}
		}
		// Destroy the bullet in all cases
		Destroy(gameObject);
	}
}
