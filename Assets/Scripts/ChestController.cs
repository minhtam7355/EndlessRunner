using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestController : MonoBehaviour
{
    public GameObject chestClose;
    public GameObject chestOpen;
    public GameObject popup;
    public TextMeshProUGUI questionText;  // use TextMeshProUGUI
    public Button[] answerButtons;

    // correct button
    private Button correctAnswerButton;

    private void Start()
    {
        // chest_open and popup is hided
        chestOpen.SetActive(false);
        popup.SetActive(false);

        // Open chest in 2s
        Invoke("OpenChest", 2f);
    }

    private void OpenChest()
    {
        // Làm rương close nhảy lên và lắc qua lắc lại
        StartCoroutine(ShakeChest());
    }

    private IEnumerator ShakeChest()
    {
        Vector3 originalPos = chestClose.transform.position;
        float elapsedTime = 0f;

        // Tạo hiệu ứng nhảy lên
        float jumpHeight = 0.5f;
        Vector3 jumpPos = originalPos + new Vector3(0, jumpHeight, 0);

        // Thời gian nhảy lên
        float jumpDuration = 0.5f;

        // Nhảy lên
        while (elapsedTime < jumpDuration)
        {
            chestClose.transform.position = Vector3.Lerp(originalPos, jumpPos, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestClose.transform.position = jumpPos;
        elapsedTime = 0f;

        // Lắc qua lắc lại khi ở không trung
        float shakeDuration = 0.5f;
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

        // Thời gian rơi xuống
        float fallDuration = 0.5f;

        // Rơi xuống
        while (elapsedTime < fallDuration)
        {
            chestClose.transform.position = Vector3.Lerp(jumpPos, originalPos, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestClose.transform.position = originalPos;

        // Sau khi rơi xuống, hiện rương open
        ShowOpenChest();
    }

    private void ShowOpenChest()
    {
        chestClose.SetActive(false);
        chestOpen.SetActive(true);

        // Hiển thị popup sau 0.5 giây
        Invoke("ShowPopup", 0.5f);
    }

    private void ShowPopup()
    {
        popup.SetActive(true);
        questionText.text = "How old are you?";

        // Gán nhãn đúng cho nút đúng
        foreach (Button btn in answerButtons)
        {
            if (btn.name == "AnswerC") // Giả sử AnswerC là đáp án đúng
            {
                correctAnswerButton = btn;
            }
            btn.onClick.AddListener(CheckAnswer);
        }
    }

    private void CheckAnswer()
    {
        Button selectedButton = (Button)UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // Kiểm tra xem nút được chọn có phải là nút đúng không
        if (selectedButton == correctAnswerButton)
        {
            popup.SetActive(false);
            Invoke("HideOpenChest", 0.5f);
        }
        else
        {
            Debug.Log("Sai rồi!");
        }
    }

    private void HideOpenChest()
    {
        chestOpen.SetActive(false);
        chestClose.SetActive(true);
    }
}