using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class HighscoreHandler : MonoBehaviour
{


    [Serializable]
    public class HighscoreEntry
    {
        public string PlayerName;
        public int Score;
    }

    [Serializable]
    public class HighscoreData
    {
        List<HighscoreEntry> highscoreList = new List<HighscoreEntry>();
    }
   
    [SerializeField] int maxCount = 4;

    void Start()
    {
       
    }


   

    
}
