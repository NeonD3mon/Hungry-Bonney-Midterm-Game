using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using myGame.data;

public class LevelClearScript : MonoBehaviour
{
    [SerializeField] TMP_Text _uiText;
    [SerializeField] float _flickerRate = 0.5f;
    public GameObject HighscoreContainer; 
    public GameObject highscoreElementPrefab; 
    private AudioSource _source;
    [SerializeField] AudioClip _uiSound;

    bool inCoroutine = false;

    Coroutine _corouting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inCoroutine = false;
        LoadAndDisplayScores();
        _source = GetComponent<AudioSource>();
       

    }

    IEnumerator Flicker()
    {
        inCoroutine = true;
        _uiText.enabled = !_uiText.enabled;

        yield return new WaitForSeconds(_flickerRate);

        inCoroutine = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCoroutine)
            _corouting = StartCoroutine(Flicker());

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _source.PlayOneShot(_uiSound);
            StopCoroutine(_corouting);
            inCoroutine = false;
            SceneManager.LoadScene("Level1");
        }
        
    }

    void LoadAndDisplayScores()
    {
        string json = PlayerPrefs.GetString("HighScores", "");
        if (string.IsNullOrEmpty(json)) return;

        HighscoreData data = JsonUtility.FromJson<HighscoreData>(json);

        data.entries.Sort((a, b) => a.time.CompareTo(b.time));

        int maxEntriesToShow = 4;
        int entriesToShow = Mathf.Min(data.entries.Count, maxEntriesToShow);


        for (int i = 0; i < entriesToShow; i++)
        {
            HighscoreEntry entry = data.entries[i];
            
            GameObject element = Instantiate(highscoreElementPrefab, HighscoreContainer.transform);
            
            

            TextMeshProUGUI[] texts = element.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = entry.playerName;
                int minutes = entry.time / 60;
                int seconds = entry.time % 60;
                texts[1].text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }
    
}
