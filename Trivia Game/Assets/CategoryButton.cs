using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public int CategoryId;
    private string _categoryName;
    public string CategoryName
    {
        get => _categoryName;
        set
        {
            _categoryName = value;
            _text.text = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
