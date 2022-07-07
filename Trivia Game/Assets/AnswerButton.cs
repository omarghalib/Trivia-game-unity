using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public bool IsCorrect;
    private string _questionText;
    public string QuestionText
    {
        get => _questionText;
        set
        {
            _questionText = value;
            _text.text = value;
        }
    }

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Debug.Log(IsCorrect);
    }
}
