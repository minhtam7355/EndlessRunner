using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "ScriptableObjects/QuestionData", order = 1)]
public class QuestionData : ScriptableObject
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public Question[] questions;

    public void ShuffleQuestions()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            Question temp = questions[i];
            int randomIndex = Random.Range(i, questions.Length);
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }
}
