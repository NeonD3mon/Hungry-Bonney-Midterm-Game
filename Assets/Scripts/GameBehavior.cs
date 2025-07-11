using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    public Utilities.GameState CurrentState;

    private GunBehavior gun;

    public Dictionary<string, int> Highscore = new();


    public string playerName;

    public int ammo;



    public int time { get; set; }

    [SerializeField] TextMeshProUGUI _scoreGUI;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] TextMeshProUGUI _ammo;

    void Awake()
{
    Debug.Log("Awake called on GameBehavior");

    if (Instance != null && Instance != this)
    {
        Debug.Log("Duplicate GameBehavior detected. Destroying.");
        Destroy(gameObject);
        return;
    }

    Debug.Log("GameBehavior instance set and preserved");
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
    void Start()
    {
        CurrentState = Utilities.GameState.Play;
        gun = GetComponent<GunBehavior>();
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    
    _ammo = GameObject.Find("Ammo")?.GetComponent<TextMeshProUGUI>();
    _timer = GameObject.Find("Timer")?.GetComponent<TextMeshProUGUI>();
    _scoreGUI = GameObject.Find("HighScores")?.GetComponent<TextMeshProUGUI>();

    
    AmmoUI(ammo);
    _timer.text = "Time: " + time;
}

    void StartTimer()
    {
        time = 0;
        InvokeRepeating("IncrementTime", 1, 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StopTimer()
    {
        CancelInvoke();
        SetHighScore();

    }



    public void SetHighScore()
    {
        string playerName = "player";


        if (Highscore.ContainsKey(playerName))
            Highscore[playerName] = Mathf.Max(Highscore[playerName], time);
        else
            Highscore.Add(playerName, time);


        var sortedScores = new List<KeyValuePair<string, int>>(Highscore);
        sortedScores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)); // Descending sort


        string leaderboard = "High Scores:\n";
        foreach (var pair in sortedScores)
        {
            leaderboard += $"{pair.Key}: {pair.Value}\n";
        }


        _scoreGUI.text = leaderboard;
        
    }

    void IncrementTime()
    {
        time++;
        _timer.text = "Time: " + time;
    }

    public void AmmoUI(int CurrentAmmo)
    {
        Debug.Log("Updating Ammo Count");
         _ammo.text = "Ammo: " + CurrentAmmo; 

    }

   
}
