using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMoney : MonoBehaviour
{
	public TextMeshProUGUI MoneyText;

	public void AddMoney(int moneyAmountToAdd)
	{
		SaveManager.instance.Money += moneyAmountToAdd;
		SaveManager.instance.Save();
		if (SceneManager.GetActiveScene().buildIndex == 1) // Assuming scene 1 has a build index of 1
		{
			UpdateMoneyText();
		}
	}

	public bool TryRemoveMoney(int moneyToRemove)
	{
		if (SaveManager.instance.Money >= moneyToRemove)
		{
			SaveManager.instance.Money -= moneyToRemove;
			SaveManager.instance.Save();
			if (SceneManager.GetActiveScene().buildIndex == 1) // Assuming scene 1 has a build index of 1
			{
				UpdateMoneyText();
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	private void UpdateMoneyText()
	{
		MoneyText.text = SaveManager.instance.Money + "$";
	}

	public void ChangeMoneyTextReference(TextMeshProUGUI newTextReference)
	{
		MoneyText = newTextReference;
		UpdateMoneyText();
	}
}