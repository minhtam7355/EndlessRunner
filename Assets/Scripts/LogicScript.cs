using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
	public GameObject BackgroundGameOver ;
    public GameObject GameOver;
    public GameObject PlayAgain;
    public GameObject ReturnMenu;
	public GameObject PauseUI;
    public Text scoreText; // Reference to the UI Text element to display the score
	public Text coinText; // Reference to the UI Text element to display current session coins
	public Text highscoreText; // Reference to the UI Text element to display the high score

	private float score; // Player's score
	public float pointsPerSecond = 10f; // Points earned per second
	private bool isGameOver = false; // Track if the game is over

	private int nextSpeedIncreaseScore = 100; // Next score milestone for speed increase
	private const float maxTimeScale = 10f; // Maximum game speed
	private const float timeScaleIncrement = 0.1f; // Increment for time scale

	private int totalCoins = 0; // Total coins collected by the player
	private int sessionCoins = 0; // Coins collected in the current game session
	private int highScore; // Player's high score

	void Start()
	{
		score = 0f; // Initialize score
		sessionCoins = 0; // Initialize session coin count
		totalCoins = PlayerPrefs.GetInt("Money", 0); // Load total coins from player prefs
		highScore = PlayerPrefs.GetInt("HighScore", 0); // Load high score from player prefs
		UpdateScoreText(); // Update the score display
		UpdateCoinText(); // Update the coin display
		UpdateHighScoreText(); // Update the high score display

		Time.timeScale = 1; // Ensure the game runs at normal speed
	}

	void Update()
	{
		if (!isGameOver)
		{
			// Increment the score based on the time passed
			score += pointsPerSecond * Time.deltaTime;
			UpdateScoreText(); // Update the score display

			// Check if the current score exceeds the high score
			if (Mathf.FloorToInt(score) > highScore)
			{
				highScore = Mathf.FloorToInt(score);
				UpdateHighScoreText(); // Update the high score display
			}

			// Check if the score has reached the next milestone
			if (score >= nextSpeedIncreaseScore)
			{
				IncreaseGameSpeed();
				nextSpeedIncreaseScore += 100; // Set the next milestone
			}
		}
	}

	private void IncreaseGameSpeed()
	{
		// Increase the game speed but ensure it does not exceed the maximum time scale
		if (Time.timeScale < maxTimeScale)
		{
			Time.timeScale += timeScaleIncrement;
			Time.timeScale = Mathf.Min(Time.timeScale, maxTimeScale);
			Debug.Log("Increased game speed. Current TimeScale: " + Time.timeScale);
		}
	}

	public void restartGame()
	{
		SceneManager.LoadSceneAsync(2);
		Time.timeScale = 1;
		isGameOver = false; // Reset game over state
		sessionCoins = 0; // Reset session coins
		score = 0f; // Reset score
		nextSpeedIncreaseScore = 100; // Reset the milestone
		UpdateCoinText(); // Update the coin displays
		UpdateScoreText();
	}
	public void returnToMenu()
	{
        SceneManager.LoadSceneAsync(0);
    }

	public void gameOver()
	{
        BackgroundGameOver.SetActive(true);
        GameOver.SetActive(true);
		PlayAgain.SetActive(true );
		ReturnMenu.SetActive(true );
		PauseUI.SetActive(false);

        isGameOver = true; // Stop score increment
		Time.timeScale = 0; // Pause the game

		UpdateTotalCoin();

		// Save the high score when the game is over
		if (Mathf.FloorToInt(score) > highScore)
		{
			highScore = Mathf.FloorToInt(score);
			PlayerPrefs.SetInt("HighScore", highScore);
			PlayerPrefs.Save();
		}
	}

	private void UpdateTotalCoin()
	{
		// Convert points to coins, 100 points = 1 coin
		int newCoins = Mathf.FloorToInt(score / 100);

		// Save the updated total coins to player prefs
		FindObjectOfType<PlayerMoney>().AddMoney(newCoins + sessionCoins);

	}
	private void UpdateCoinText()
	{
		coinText.text = "Coins: " + sessionCoins.ToString();
	}

	private void UpdateScoreText()
	{
		scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
	}
	private void UpdateHighScoreText()
	{
		highscoreText.text = "High Score: " + highScore.ToString();
	}

	public void CollectCoin()
	{
		sessionCoins++;
		UpdateCoinText();
	}
}
