using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public GameObject GameOverScene;
    public Text scoreText; // Reference to the UI Text element to display the score
    public Text coinText; // Reference to the UI Text element to display the coins
    public Text highscoreText; // Reference to the UI Text element to display the high score
    private float score; // Player's score
    public float pointsPerSecond = 10f; // Points earned per second
    private bool isGameOver = false; // Track if the game is over
    private bool hasConvertedPointsToCoins = false; // Ensure coins are only converted once

    private int nextSpeedIncreaseScore = 100; // Next score milestone for speed increase
    private const float maxTimeScale = 10f; // Maximum game speed
    private const float timeScaleIncrement = 0.1f; // Increment for time scale

    private static int totalCoins = 0; // Total coins collected by the player
    private int highScore; // Player's high score

    void Start()
    {
        score = 0f; // Initialize score
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Load total coins from player prefs
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
                PlayerPrefs.SetInt("HighScore", highScore); // Save the high score
                PlayerPrefs.Save();
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
        SceneManager.LoadScene("Main Scene");
        Time.timeScale = 1;
        isGameOver = false; // Reset game over state
        hasConvertedPointsToCoins = false; // Reset coin conversion state
        score = 0f; // Reset score
        nextSpeedIncreaseScore = 100; // Reset the milestone
    }

    public void gameOver()
    {
        if (!isGameOver)
        {
            GameOverScene.SetActive(true);
            isGameOver = true; // Stop score increment
            Time.timeScale = 0; // Pause the game

            // Convert points to coins only if it hasn't been done yet
            if (!hasConvertedPointsToCoins)
            {
                ConvertPointsToCoins();
                hasConvertedPointsToCoins = true; // Mark that points have been converted
            }

            // Save the high score when the game is over
            if (Mathf.FloorToInt(score) > highScore)
            {
                highScore = Mathf.FloorToInt(score);
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }
        }
    }

    private void ConvertPointsToCoins()
    {
        // Convert points to coins, 100 points = 1 coin
        int newCoins = Mathf.FloorToInt(score / 100);
        totalCoins += newCoins;

        // Save the updated total coins to player prefs
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();

        // Update the coin text UI element
        UpdateCoinText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + totalCoins.ToString();
    }

    private void UpdateHighScoreText()
    {
        highscoreText.text = "High Score: " + highScore.ToString();
    }
}
