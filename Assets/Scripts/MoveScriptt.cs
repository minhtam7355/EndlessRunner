using System.Collections;
using UnityEngine;

public class MoveScriptt : MonoBehaviour
{
	private DeathTrigger _deathTrigger;

	void Start()
	{
		StartCoroutine(WaitForPlayer());
	}

	private IEnumerator WaitForPlayer()
	{
		GameObject playerObject = null;
		while (playerObject == null)
		{
			playerObject = GameObject.FindGameObjectWithTag("Player");
			yield return null; // Wait for the next frame
		}

		_deathTrigger = playerObject.GetComponent<DeathTrigger>();
	}

	void Update()
	{
		if (_deathTrigger != null && !_deathTrigger.IsDead)
		{
			transform.position += new Vector3(0, 0, -10) * Time.deltaTime;
		}
	}
}
