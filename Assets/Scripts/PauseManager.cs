using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	// Start is called before the first frame update
	public static bool isPaused = false;
	public GameObject pauseMenu;

	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
	public void Resume()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}
	public void Home()
	{
		SceneManager.LoadSceneAsync(0);
	}
	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
