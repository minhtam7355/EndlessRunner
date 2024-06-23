using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public GameObject chestClose;
    public GameObject chestOpen;
    public GameObject popup;
    public GameObject chestPrefab;
    public Transform ChestTransform;
    public QuestionData questionData; // Reference to your QuestionData asset

    public Text questionText;
    public Button[] answerButtons; // Array of buttons for answers

    private bool canSpawnChest = true;
    private bool isGamePaused = false;
    private GameObject currentChest; // Reference to the current chest

    private void Start()
    {
        chestOpen.SetActive(false);
        popup.SetActive(false);

        // Shuffle questions at the start
        questionData.ShuffleQuestions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentChest = gameObject; // Set the current chest to this game object
            OpenChest();

            if (canSpawnChest)
            {
                SpawnChest();
                canSpawnChest = false;
            }
        }
    }

    private void OpenChest()
    {
        StartCoroutine(ShakeChest());
    }

    private void SpawnChest()
    {
        if (ChestTransform == null)
        {
            Debug.LogError("ChestTransform is not assigned!");
            return;
        }

        if (chestPrefab == null)
        {
            Debug.LogError("chestPrefab is not assigned!");
            return;
        }

        Vector3[] possiblePositions = new Vector3[]
        {
            new Vector3(-1, 1, ChestTransform.position.z + 80),
            new Vector3(4, 1, ChestTransform.position.z + 80),
            new Vector3(-6, 1, ChestTransform.position.z + 80)
        };

        Vector3 randomPosition = possiblePositions[Random.Range(0, possiblePositions.Length)];

        Instantiate(chestPrefab, randomPosition, Quaternion.identity);
    }

    private IEnumerator ShakeChest()
    {
        Vector3 originalPos = chestClose.transform.position;
        float elapsedTime = 0f;

        float jumpHeight = 0.5f;
        Vector3 jumpPos = originalPos + new Vector3(0, jumpHeight, 0);
        float jumpDuration = 0.2f;

        while (elapsedTime < jumpDuration)
        {
            chestClose.transform.position = Vector3.Lerp(originalPos, jumpPos, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestClose.transform.position = jumpPos;
        elapsedTime = 0f;

        float shakeDuration = 0.2f;
        float shakeAmount = 0.1f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            chestClose.transform.position = new Vector3(jumpPos.x + x, jumpPos.y, jumpPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestClose.transform.position = jumpPos;
        elapsedTime = 0f;

        float fallDuration = 0.2f;

        while (elapsedTime < fallDuration)
        {
            chestClose.transform.position = Vector3.Lerp(jumpPos, originalPos, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestClose.transform.position = originalPos;

        ShowOpenChest();
    }

    private void ShowOpenChest()
    {
        
        isGamePaused = true;

        chestClose.SetActive(false);
        chestOpen.SetActive(true);
        Time.timeScale = 0f;
        popup.SetActive(true);

        // Pick a random question from the shuffled list
        QuestionData.Question randomQuestion = questionData.questions[Random.Range(0, questionData.questions.Length)];

        questionText.text = randomQuestion.questionText;

        // Display answers without shuffling them
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < randomQuestion.answers.Length)
            {
                answerButtons[i].GetComponentInChildren<Text>().text = randomQuestion.answers[i];

                // Clear previous listeners to prevent multiple additions
                answerButtons[i].onClick.RemoveAllListeners();

                if (i == randomQuestion.correctAnswerIndex)
                {
                    answerButtons[i].onClick.AddListener(CorrectAnswerSelected);
                }
                else
                {
                    answerButtons[i].onClick.AddListener(IncorrectAnswerSelected);
                }
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void CorrectAnswerSelected()
    {
        Debug.Log("Correct answer!");
        EndPopup();
    }

    private void IncorrectAnswerSelected()
    {
        Debug.Log("Incorrect answer!");
        EndPopup();
    }

    private void EndPopup()
    {
        popup.SetActive(false);

        Time.timeScale = 1f;
        isGamePaused = false;

        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        // Destroy the entire chest GameObject
        Destroy(currentChest);

        HideOpenChest();
    }

    private void HideOpenChest()
    {
        chestOpen.SetActive(false);
        Destroy(chestClose);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    // Removed the ShuffleAnswers method as it's no longer needed
}
