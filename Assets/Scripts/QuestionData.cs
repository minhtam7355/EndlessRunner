using UnityEngine;

[CreateAssetMenu(fileName = "New Question Data", menuName = "Question Data", order = 51)]
public class QuestionData : ScriptableObject
{
    [System.Serializable]
    public struct Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex; // Index of the correct answer in the answers array
    }

    public Question[] questions;
}
