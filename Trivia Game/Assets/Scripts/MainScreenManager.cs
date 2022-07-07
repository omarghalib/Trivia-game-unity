using System;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] private Button _chooseCategoryButton;

    [SerializeField] private Button _PlayRandomButton;

    [SerializeField] private Button _PlayWithCategoryButton;

    [SerializeField] private TextMeshProUGUI _PlayWithCategoryButtonText;
    // Start is called before the first frame update
    private void Start()
    {
        _chooseCategoryButton.onClick.AddListener(() => { SceneManager.LoadScene("CategoryListScreenScene"); });
        _PlayRandomButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("category_choice_name", "Random");
            SceneManager.LoadScene("GameScreenScene");
        });
        if (PlayerPrefs.HasKey("category_choice_name") && PlayerPrefs.GetString("category_choice_name") != "Random")
        {
            _PlayWithCategoryButton.gameObject.SetActive(true);
            _PlayWithCategoryButtonText.text = "Play "+PlayerPrefs.GetString("category_choice_name");
        }
        else
        {
            _PlayWithCategoryButton.gameObject.SetActive(false);
        }
        _PlayWithCategoryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScreenScene");
        });
    }

    // Update is called once per frame
    private void Update()
    {
    }
}