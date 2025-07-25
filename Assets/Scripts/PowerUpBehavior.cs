using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    [SerializeField] Collider2D PowerUpCollider;
    public GameBehavior game;
    SoundManagerScript soundManager;
    public bool IsTaken;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PowerUpCollider = GetComponent<Collider2D>();
        IsTaken = false;
        soundManager = Object.FindFirstObjectByType<SoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Player"))
    {
        if (GameBehavior.Instance == null)
        {
            Debug.LogWarning("PowerUp: GameBehavior not ready yet!");
            return;
        }
        IsTaken = true;
        soundManager._oneShots.PlayOneShot(soundManager._coockiePowerUp);
        GameBehavior.Instance.time -= 5;
        Debug.Log("PowerUp triggered!");
        IsTaken = false;
        Destroy(gameObject);
    }
    
}
}
