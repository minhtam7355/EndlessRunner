using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
	[SerializeField] private GameObject loadingScreen;
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI progressText; // Added TextMeshProUGUI field

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		loadingScreen.SetActive(true);
		mainMenu.SetActive(false);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			slider.value = progress;
			progressText.text = (progress * 100).ToString("F0") + "%"; // Update progress text

			yield return null;
		}
	}
}
