using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
public class PauseMenuScript : MonoBehaviour
{
    public Utilities.GameState CurrentState { get; set; }
    GameBehavior game;
    [SerializeField] Button _quitButton;
    [SerializeField] Button _menuButton;
    bool toggleMenu = false;
    private AudioSource _source;
    [SerializeField] AudioClip uiAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = Utilities.GameState.Pause;
        game = Object.FindAnyObjectByType<GameBehavior>();
        // _quitButton = GetComponent<Button>();
        // _menuButton = GetComponent<Button>();
        _source = GetComponent<AudioSource>();
        _quitButton.onClick.AddListener(QuitGame);
        _menuButton.onClick.AddListener(BackToMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void QuitGame()
    {
#if UNITY_EDITOR
        //function runs when the game is running from unity
        _source.PlayOneShot(uiAudio);
        EditorApplication.isPlaying = false;
#else
        // Function runs when the game is running from build
        _source.PlayOneShot(uiAudio);
        Application.Quit();
#endif
    }

    void BackToMenu()
    {
        _source.PlayOneShot(uiAudio);
        if (GameBehavior.Instance != null)
        {
            GameBehavior.Instance.CurrentState = Utilities.GameState.Play;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync("PauseMenu");
    }
}

