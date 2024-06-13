using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public GameObject GameOverScene;
    public Text scoreText; // Reference to the UI Text element to display the score
    private float score; // Player's score
    public float pointsPerSecond = 10f; // Points earned per second
    private bool isGameOver = false; // Track if the game is over

    private int nextSpeedIncreaseScore = 100; // Next score milestone for speed increase
    private const float maxTimeScale = 10f; // Maximum game speed
    private const float timeScaleIncrement = 0.1f; // Increment for time scale

    void Start()
    {
        score = 0f; // Initialize score
        UpdateScoreText(); // Update the score display
        Time.timeScale = 1; // Ensure the game runs at normal speed
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Increment the score based on the time passed
            score += pointsPerSecond * Time.deltaTime;
            UpdateScoreText(); // Update the score display

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
        score = 0f; // Reset score
        nextSpeedIncreaseScore = 100; // Reset the milestone
    }

    public void gameOver()
    {
        GameOverScene.SetActive(true);
        isGameOver = true; // Stop score increment
        Time.timeScale = 0; // Pause the game
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }
}
