using System;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainScreenManager : MonoBehaviour
    {
        [SerializeField] private Button _chooseCategoryButton;

        [SerializeField] private Button _PlayRandomButton;

        [SerializeField] private Button _PlayWithCategoryButton;
        // Start is called before the first frame update
        void Start()
        {
            _chooseCategoryButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("CategoryListScreenScene");
            });
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}