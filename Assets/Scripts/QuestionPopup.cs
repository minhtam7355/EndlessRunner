using UnityEngine;
using UnityEngine.UI;

public class QuestionPopup : MonoBehaviour
{
    public Text questionText;
    public Button[] answerButtons;
    public string correctAnswer;
    public GameObject chestClosePrefab;
    public Transform chestPosition;

    private void Start()
    {
        questionText.text = "Câu hỏi: ...";
        answerButtons[0].GetComponentInChildren<Text>().text = "A. Đáp án A";
        answerButtons[1].GetComponentInChildren<Text>().text = "B. Đáp án B";
        answerButtons[2].GetComponentInChildren<Text>().text = "C. Đáp án C";
        answerButtons[3].GetComponentInChildren<Text>().text = "D. Đáp án D";

        foreach (var button in answerButtons)
        {
            button.onClick.AddListener(() => CheckAnswer(button));
        }
    }

    private void CheckAnswer(Button selectedButton)
    {
        if (selectedButton.GetComponentInChildren<Text>().text == correctAnswer)
        {
            // Nếu đúng, ẩn rương mở và hiện rương đóng
            Destroy(GameObject.FindWithTag("ChestOpen"));
            Instantiate(chestClosePrefab, chestPosition.position, chestPosition.rotation);
        }
        else
        {
            // Nếu sai, xử lý khác nếu cần
            Debug.Log("Sai rồi!");
        }

        gameObject.SetActive(false);
    }
}
