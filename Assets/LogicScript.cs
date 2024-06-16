using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
	public GameObject GameOverScene;
	public void restartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;

	}
	public void gameOver()
	{
		GameOverScene.SetActive(true);
	}
}
