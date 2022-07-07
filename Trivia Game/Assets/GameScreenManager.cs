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
    [SerializeField] private TextMeshProUGUI _actionButtonText;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private TextMeshProUGUI _questionRankText;
    [SerializeField] private TextMeshProUGUI _questionCategory;
    [SerializeField] private TextMeshProUGUI _counterText;
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
        ShowNextQuestion();
    }

    private void UpdateQuestionRank(int rank)
    {
        _questionRankText.text = "Question " + rank + 1 + " / " + ApiClient.FetchedQuestions.Count;
    }
    
    private void UpdateQuestionText(int rank)
    {
        _questionText.text = ApiClient.FetchedQuestions[rank].question;
    }

    private void UpdateQuestionCategory(int rank)
    {
        _questionCategory.text = ApiClient.FetchedQuestions[rank].category;
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

    private void ShowNextQuestion()
    {
        if (_questionRank < ApiClient.FetchedQuestions.Count)
        {
            ClearScrollArea();
            UpdateQuestionRank(_questionRank);
            UpdateQuestionText(_questionRank);
            UpdateQuestionCategory(_questionRank);
            AddAnswersForQuestion(_questionRank);
            InitActionButton();
            _questionRank++;
            StartCoroutine(CountDownThenShowNextQuestion(60));
        }
        else
        {
            _actionButton.enabled = false;
        }
    }

    private void InitActionButton()
    {
        _actionButton.onClick.AddListener(DisplayAnswer);
        _actionButtonText.text = "Display Answer";
        UpdateButtonColor(_actionButton, new Color(0.85f, 0.855f, 0.85f));
        _actionButton.enabled = true;
    }

    private void UpdateButtonColor(Button button, Color newColor)
    {
        ColorBlock buttonColors = button.gameObject.GetComponent<Button>().colors;
        buttonColors.normalColor = newColor;
        button.gameObject.GetComponent<Button>().colors = buttonColors;
    }

    private IEnumerator CountDownThenShowNextQuestion(float seconds)
    {
        float elapsedTime = 0;
        _actionButton.enabled = false;
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            _counterText.text =(int) (seconds - elapsedTime) + " till the next question";
            yield return new WaitForEndOfFrame();
        }
        DisplayAnswer();
        StartCoroutine(CountDownInActionButtonThenShowNext(3));
    }
    
    private IEnumerator WaitForSecondsThenCountDown(float seconds)
    {
        float elapsedTime = 0;
        _actionButton.enabled = false;
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(CountDownInActionButtonThenShowNext(3));
    }
    
    private IEnumerator CountDownInActionButtonThenShowNext(float seconds)
    {
        float elapsedTime = 0;
        _actionButton.enabled = false;
        _actionButtonText.text = "Next Question In 3 Seconds";
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            _actionButtonText.text =
                "Next Question In " + (int) (seconds - elapsedTime) + " Seconds";
            yield return new WaitForEndOfFrame();
        }
        ShowNextQuestion();
    }

    public void AnswerClicked(bool isCorrectAnswer)
    {
        DisplayAnswer();
        if (isCorrectAnswer)
        {
            _actionButtonText.text = "Correct!";
            UpdateButtonColor(_actionButton, new Color(0, 1, 0));
        }
        else
        {
            _actionButtonText.text = "Wrong!";
            UpdateButtonColor(_actionButton, new Color(1, 0, 0));
        }
        StartCoroutine(WaitForSecondsThenCountDown(1));
    }

    private void DisplayAnswer()
    {
        DisableAllAnswers();
        for (int i = 0; i < _contentContainer.childCount; i++)
        {
            if (_contentContainer.GetChild(i).gameObject.GetComponent<AnswerButton>().IsCorrect)
                UpdateButtonColor(_contentContainer.GetChild(i).gameObject.GetComponent<Button>(),
                    new Color(0,1,0));
        }
    }

    private void DisableAllAnswers()
    {
        for (int i = 0; i < _contentContainer.childCount; i++)
        {
            _contentContainer.GetChild(i).gameObject.GetComponent<Button>().enabled = false;
        }
    }
}
