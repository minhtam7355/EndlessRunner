using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characterPrefabs;
	public Transform spawnPoint;
	public GameObject bulletPrefab; // Add this line to hold the bullet prefab

	void Start()
	{
		int selectedCharacter = PlayerPrefs.GetInt("CurrentCharacter");
		GameObject prefab = characterPrefabs[selectedCharacter];
		GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
		// Get the Character component and set the bullet prefab
		Character characterScript = clone.GetComponent<Character>();
		if (characterScript != null)
		{
			characterScript.bulletPrefab = bulletPrefab;
		}
	}
}
