using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characterPrefabs;
	public Transform spawnPoint;

	void Start()
	{
		int selectedCharacter = PlayerPrefs.GetInt("CurrentCharacter");
		GameObject prefab = characterPrefabs[selectedCharacter];
		GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
	}
}
