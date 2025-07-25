using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using myGame.data;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    public Utilities.GameState CurrentState { get; set; }

    private GunBehavior gun;

    public Dictionary<string, int> Highscore = new();


    public string playerName;

    public int ammo;
    int minutes;
    int seconds;



   private int _time;

    public int time
    {
        get => _time;
        set
        {
            _time = Mathf.Max(0, value); // Prevent negative time
            minutes = Mathf.FloorToInt(_time / 60);
            seconds = _time % 60;

            if (_timer != null)
                _timer.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
}

    public TextMeshProUGUI _scoreGUI;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] TextMeshProUGUI _ammo;

    GunBehavior Magazine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("GameBehavior set as Singleton");
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        CurrentState = Utilities.GameState.Play;
       
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentState == Utilities.GameState.Play)
        {
            Time.timeScale = 0f;
            CurrentState = Utilities.GameState.Pause;
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);

        }

        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            CurrentState = Utilities.GameState.Play;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
        
            
        
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
        Debug.Log($"PlayerName loaded: {playerName}");


        gun = Object.FindAnyObjectByType<GunBehavior>();
        StartCoroutine(DelayedFindUI());
    
    IEnumerator DelayedFindUI()
{
    yield return null; // wait one frame
    _ammo = GameObject.Find("Ammo")?.GetComponent<TextMeshProUGUI>();
    _timer = GameObject.Find("Timer")?.GetComponent<TextMeshProUGUI>();
    _scoreGUI = GameObject.Find("HighScores")?.GetComponent<TextMeshProUGUI>();

    if (_ammo == null) Debug.LogWarning("Ammo UI not found in scene.");
    if (_timer == null) Debug.LogWarning("Timer UI not found in scene.");
    if (_scoreGUI == null) Debug.LogWarning("HighScores UI not found in scene.");

    AmmoUI(ammo);
    _timer.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    StartTimer();
}
}

    void StartTimer()
    {   
        CancelInvoke("IncrementTime");
        time = 0;
        InvokeRepeating("IncrementTime", 1, 1);
    }

    public void RestartLevel()
    {
        CancelInvoke();
        time = 0;
        ammo = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void StopTimer()
    {
        if (IsInvoking("IncrementTime"))
        {
            CancelInvoke();
            SaveHighScore(playerName, time);  
        }
        

    }


    public void SaveHighScore(string playerName, int time)
    {
        string json = PlayerPrefs.GetString("HighScores", "");
        HighscoreData data = string.IsNullOrEmpty(json) ? new HighscoreData() : JsonUtility.FromJson<HighscoreData>(json);
       
        data.entries.Add(new HighscoreEntry { playerName = playerName, time = time });

        data.entries.Sort((a, b) => b.time.CompareTo(a.time));
        if (data.entries.Count > 10)
            data.entries = data.entries.GetRange(0, 10);

        PlayerPrefs.SetString("HighScores", JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }





    void IncrementTime()
    {
        time++;
    }

    public void AmmoUI(int CurrentAmmo)
    {
        Debug.Log("Updating Ammo Count");
        _ammo.text = "Ammo: " + CurrentAmmo;

    }



}

namespace myGame.data
{
    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int time;
    }
    [System.Serializable]
    public class HighscoreData
    {
        public List<HighscoreEntry> entries = new();
    }
    
    
}
