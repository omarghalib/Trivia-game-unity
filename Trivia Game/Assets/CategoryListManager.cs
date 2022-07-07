using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CategoryListManager : MonoBehaviour
{
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GameObject _categoryButtonPrefab;

    [SerializeField] private ScrollRect _scrollView;

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var category in ApiClient.GetCategories()) AddCategoryButtonToScrollArea(category.id, category.name);
    }

    private void AddCategoryButtonToScrollArea(int categoryId, string categoryName)
    {
        _scrollView.gameObject.SetActive(true);
        var itemGo = Instantiate(_categoryButtonPrefab);
        itemGo.GetComponent<CategoryButton>().CategoryId = categoryId;
        itemGo.GetComponent<CategoryButton>().CategoryName = categoryName;
        //parent the item to the content container
        itemGo.transform.SetParent(_contentContainer);
        //reset the item's scale -- this can get munged with UI prefabs
        itemGo.transform.localScale = Vector2.one;
        itemGo.GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("category_choice_id", categoryId);
            PlayerPrefs.SetString("category_choice_name", categoryName);
            SceneManager.LoadScene("GameScreenScene");
        });
    }

    private void ClearScrollArea()
    {
        for (var i = 0; i < _contentContainer.childCount; i++) Destroy(_contentContainer.GetChild(i).gameObject);
    }
}