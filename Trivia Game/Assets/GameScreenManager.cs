using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;
public class GameScreenManager : MonoBehaviour
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private TextMeshProUGUI _questionRankText;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameObject _answerButtonPrefab;
    [SerializeField] private ScrollRect _scrollView;
    private int _questionRank;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("category_choice_name","Random")!="Random")
            ApiClient.FetchQuestions(PlayerPrefs.GetInt("category_choice_id"));
        else
        {
            ApiClient.FetchQuestions();
        }
        UpdateQuestionRank(_questionRank);
        UpdateQuestionText(_questionRank);
        AddAnswersForQuestion(_questionRank);
    }

    private void UpdateQuestionRank(int rank)
    {
        _questionRankText.text = "Question " + rank + " / " + ApiClient.FetchedQuestions.Count;
    }
    
    private void UpdateQuestionText(int rank)
    {
        _questionText.text = ApiClient.FetchedQuestions[rank].question;
    }

    private void AddAnswersForQuestion(int rank)
    {
        Random rand = new Random();
        int correctAnswerIndex = rand.Next(ApiClient.FetchedQuestions[rank].incorrect_answers.Count);
        for (int i = 0; i < ApiClient.FetchedQuestions[rank].incorrect_answers.Count; i++)
        {
            if(i != correctAnswerIndex)
                AddAnswerButtonToScrollArea(false, ApiClient.FetchedQuestions[rank].incorrect_answers[i]);
            else
            {
                AddAnswerButtonToScrollArea(true, ApiClient.FetchedQuestions[rank].correct_answer);
                AddAnswerButtonToScrollArea(false, ApiClient.FetchedQuestions[rank].incorrect_answers[i]);
            }
        }
        if(correctAnswerIndex == ApiClient.FetchedQuestions[rank].incorrect_answers.Count)
            AddAnswerButtonToScrollArea(true, ApiClient.FetchedQuestions[rank].correct_answer);
    }
    private void AddAnswerButtonToScrollArea(bool isCorrect, string question)
    {
        _scrollView.gameObject.SetActive(true);
        var itemGo = Instantiate(_answerButtonPrefab);
        itemGo.GetComponent<AnswerButton>().IsCorrect = isCorrect;
        itemGo.GetComponent<AnswerButton>().QuestionText = question;
        //parent the item to the content container
        itemGo.transform.SetParent(_contentContainer);
        //reset the item's scale -- this can get munged with UI prefabs
        itemGo.transform.localScale = Vector2.one;
    }

    private void ClearScrollArea()
    {
        for (int i = 0; i < _contentContainer.childCount; i++)
        {
            Destroy(_contentContainer.GetChild(i).gameObject);
        }
    }
}
