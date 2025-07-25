using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenScript : MonoBehaviour
{
    [SerializeField] TMP_Text _uiText;
    [SerializeField] float _flickerRate = 0.5f;
    [SerializeField] TMP_Text _querry;
    [SerializeField] TMP_Text _tutorial;
    Utilities.GameState CurrentState;
    public TMP_InputField nameInputField;
    
    public GameObject inputBackground;
    private AudioSource _source;
    [SerializeField] AudioClip uiAudio;

    private bool inCoroutine = false;
    private Coroutine _coroutine;
    private bool waitingForName = false;

    void Start()
    {
        
        CurrentState = Utilities.GameState.Play;

        _source = GetComponent<AudioSource>();
        inCoroutine = false;
        nameInputField.gameObject.SetActive(false);
        _querry.enabled = false;
        _tutorial.enabled = false;
        inputBackground.SetActive(false);
       //PlayerPrefs.DeleteAll();
       //PlayerPrefs.Save();

    }

    IEnumerator Flicker()
    {
        inCoroutine = true;
        _uiText.enabled = !_uiText.enabled;
        yield return new WaitForSeconds(_flickerRate);
        inCoroutine = false;
    }

    void Update()
    {
        if (!inCoroutine)
            _coroutine = StartCoroutine(Flicker());

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _source.PlayOneShot(uiAudio);
            if (!waitingForName)
            {
                // First press: show input field
                _querry.enabled = true;
                _tutorial.enabled = true;
                inputBackground.SetActive(true);
                waitingForName = true;
                nameInputField.gameObject.SetActive(true);
                nameInputField.Select(); // Focus the input field
                nameInputField.text = ""; // Clear previous input
                _uiText.enabled = false; // Hide "Press Return" text
                StopCoroutine(_coroutine);
                inCoroutine = false;
            }
            else
            {
                // Second press: check name and start game
                string playerName = nameInputField.text;

                if (!string.IsNullOrEmpty(playerName))
                {
                    PlayerPrefs.SetString("PlayerName", playerName);
                    SceneManager.LoadScene("Level1");
                }
            }
        }
    }
}
