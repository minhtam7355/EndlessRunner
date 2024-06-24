using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
	public static SaveManager instance { get; private set; }

	public int CurrentCharacter;
	public int Money;
	public int HighScore;
	public bool[] CharactersUnlocked = new bool[4] { true, false, false, false };

	private void Awake()
	{
		if (instance != null && instance != this)
			Destroy(gameObject);
		else
			instance = this;

		DontDestroyOnLoad(gameObject);
		Load();
	}

	public void Load()
	{
		// Load money
		Money = PlayerPrefs.GetInt("Money", 0); // Default to 0 if not found

		// Load current character
		CurrentCharacter = PlayerPrefs.GetInt("CurrentCharacter", 0); // Default to 0 if not found

		// Load high score
		HighScore = PlayerPrefs.GetInt("HighScore", 0); // Default to 0 if not found

		// Load characters unlocked
		for (int i = 0; i < CharactersUnlocked.Length; i++)
		{
			CharactersUnlocked[i] = PlayerPrefs.GetInt("CharactersUnlocked" + i, i == 0 ? 1 : 0) == 1;
		}
	}

	public void Save()
	{
		// Save money
		PlayerPrefs.SetInt("Money", Money);

		// Save current character
		PlayerPrefs.SetInt("CurrentCharacter", CurrentCharacter);

		// Save high score
		PlayerPrefs.SetInt("HighScore", HighScore);

		// Save characters unlocked
		for (int i = 0; i < CharactersUnlocked.Length; i++)
		{
			PlayerPrefs.SetInt("CharactersUnlocked" + i, CharactersUnlocked[i] ? 1 : 0);
		}

		// Ensure all PlayerPrefs are saved
		PlayerPrefs.Save();
	}
	public void ResetProgress()
	{
		// Reset all saved values to their default states
		Money = 5000;
		CurrentCharacter = 0;
		HighScore = 0;
		CharactersUnlocked = new bool[4] { true, false, false, false };

		// Save the reset values
		Save();
	}
}
