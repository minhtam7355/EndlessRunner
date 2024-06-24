using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characterPrefabs;
	public Transform spawnPoint;
	public GameObject[] bulletPrefabs; // Array to hold different bullet prefabs

	void Start()
	{
		int selectedCharacter = PlayerPrefs.GetInt("CurrentCharacter");
		GameObject prefab = characterPrefabs[selectedCharacter];
		GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
		// Get the Character component and set the bullet prefab
		Character characterScript = clone.GetComponent<Character>();
		if (characterScript != null)
		{
			// Assign the corresponding bullet prefab based on selected character
			if (selectedCharacter == 0)
			{
				characterScript.bulletPrefab = bulletPrefabs[0]; // Use bullet1 for character 0
			}
			else if (selectedCharacter == 1 || selectedCharacter == 2 || selectedCharacter == 3)
			{
				characterScript.bulletPrefab = bulletPrefabs[1]; // Use bullet2 for character 1, 2 ,3
			}
		}
	}
}
