using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public static class ApiClient
{
    [Serializable]
    public class CategoryList
    {
        public List<Category> trivia_categories;
    }

    [Serializable]
    public class Category
    {
        public int id;
        public string name;
    }
    public static List<Category> GetCategories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
        string jsonResponse = reader.ReadToEnd();
        return JsonUtility.FromJson<CategoryList>(jsonResponse).trivia_categories;
    }
    
    
}