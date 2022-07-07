using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public static class ApiClient
{
    public static List<Question> FetchedQuestions;
    
    [Serializable]
    public class CategoryResponse
    {
        public List<Category> trivia_categories;
    }

    [Serializable]
    public class Category
    {
        public int id;
        public string name;
    }
    
    [Serializable]
    public class QuestionsResponse
    {
        public List<Question> results;
    }
    
    [Serializable]
    public class Question
    {
        public string question;
        public string correct_answer;
        public List<string> incorrect_answers;
        public string category;
    }
    public static List<Category> GetCategories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
        string jsonResponse = reader.ReadToEnd();
        return JsonUtility.FromJson<CategoryResponse>(jsonResponse).trivia_categories;
    }

    public static void FetchQuestions(int categoryId)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api.php?amount=10&category="+categoryId+"&type=multiple");
        ProcessQuestionsRequest(request);
    }
    
    public static void FetchQuestions()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api.php?amount=10&type=multiple");
        ProcessQuestionsRequest(request);
    }

    private static void ProcessQuestionsRequest(HttpWebRequest request)
    {
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
        string jsonResponse = reader.ReadToEnd();
        FetchedQuestions = JsonUtility.FromJson<QuestionsResponse>(jsonResponse).results;
    }
}