using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainScreenManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            API_Client.GetCategories();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}