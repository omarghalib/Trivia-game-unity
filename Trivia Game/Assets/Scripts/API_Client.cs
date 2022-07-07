using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public static class API_Client
{
    [Serializable]
    public class CategoryDictionary
    {
        public List<Category> trivia_categories;
    }

    [Serializable]
    public class Category
    {
        public int id;
        public string name;
    }
    public static void GetCategories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
        string jsonResponse = reader.ReadToEnd();
        CategoryDictionary categories = JsonUtility.FromJson<CategoryDictionary>(jsonResponse);
        Debug.Log("jsonResponse: "+jsonResponse);
        Debug.Log(categories.trivia_categories.Count);
        foreach (var category in categories.trivia_categories)
        {
            Debug.Log(category.id);
            Debug.Log(category.name);
        }
    }
}