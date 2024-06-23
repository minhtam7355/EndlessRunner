using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerMoneyDisplay : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(FindAndAssignPlayerMoney());
	}

	private IEnumerator FindAndAssignPlayerMoney()
	{
		PlayerMoney playerMoney = null;
		while (playerMoney == null)
		{
			playerMoney = FindObjectOfType<PlayerMoney>();
			yield return null; // Wait for the next frame
		}

		TextMeshProUGUI moneyTextComponent = GetComponent<TextMeshProUGUI>();
		if (moneyTextComponent != null)
		{
			playerMoney.ChangeMoneyTextReference(moneyTextComponent);
		}
		else
		{
			Debug.LogError("TextMeshProUGUI component not found.");
		}
	}
}
