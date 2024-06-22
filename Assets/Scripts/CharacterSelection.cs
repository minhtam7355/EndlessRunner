using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	[SerializeField] public TMPro.TMP_Text characterName;

	[Header("Play/Buy Buttons")]
	[SerializeField] private Button play;
	[SerializeField] private Button buy;
	[SerializeField] private TMPro.TMP_Text priceText;

	[Header("Character Attributes")]
	[SerializeField] private int[] characterPrices;

	private SaveManager saveManager;

	private void Awake()
	{
		saveManager = SaveManager.instance;
	}

	private void Start()
	{
		UpdateCharacterDisplay();
		UpdateUI();
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("CurrentCharacter", saveManager.CurrentCharacter);
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}

	void Update()
	{
		characterName.text = characters[saveManager.CurrentCharacter].name;
	}

	public void NextCharacter()
	{
		characters[saveManager.CurrentCharacter].SetActive(false);
		saveManager.CurrentCharacter = (saveManager.CurrentCharacter + 1) % characters.Length;
		characters[saveManager.CurrentCharacter].SetActive(true);
		UpdateCharacterDisplay();
		UpdateUI();
	}

	public void PreviousCharacter()
	{
		characters[saveManager.CurrentCharacter].SetActive(false);
		saveManager.CurrentCharacter--;
		if (saveManager.CurrentCharacter < 0)
		{
			saveManager.CurrentCharacter += characters.Length;
		}
		characters[saveManager.CurrentCharacter].SetActive(true);
		UpdateCharacterDisplay();
		UpdateUI();
	}

	private void UpdateCharacterDisplay()
	{
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].SetActive(i == saveManager.CurrentCharacter);
		}
		characterName.text = characters[saveManager.CurrentCharacter].name;
	}

	private void UpdateUI()
	{
		int currentCharacter = saveManager.CurrentCharacter;

		// If current character is unlocked, show the play button
		if (saveManager.CharactersUnlocked[saveManager.CurrentCharacter])
		{
			play.gameObject.SetActive(true);
			buy.gameObject.SetActive(false);
		}
		// If not, show the buy button and set the price
		else
		{
			play.gameObject.SetActive(false);
			buy.gameObject.SetActive(true);
			priceText.text = characterPrices[saveManager.CurrentCharacter] + "$";
		}
	}
	public void BuyCharacter()
	{
		if (FindObjectOfType<PlayerMoney>().TryRemoveMoney(characterPrices[saveManager.CurrentCharacter]))
		{
			saveManager.CharactersUnlocked[saveManager.CurrentCharacter] = true;
			saveManager.Save();
			UpdateUI();
		}
	}
}
