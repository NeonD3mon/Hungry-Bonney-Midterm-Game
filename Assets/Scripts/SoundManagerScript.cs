using System.Collections;
using System.Data.Common;
using System.Diagnostics;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    
    private PlayerInputController input;
    private GroundCheck groundCheck;
    private EnemySlimeBehavior slime;
    private PowerUpBehavior powerUp;
    public AudioClip _playerJump;
    public AudioClip _playerRun;
    public AudioClip _coockiePowerUp;
    private bool hasPlayedJumpSound = false;


    private AudioSource _source;
    public AudioSource _oneShots;
    Coroutine footstepLoopCoroutine;

    IEnumerator FootstepLoop(AudioClip clip)
    {
        _source.clip = clip;
        _source.loop = true;
        _source.Play();

        // Keep playing as long as player is moving AND grounded
        while ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && groundCheck.isGrounded)
        {
            yield return null;
        }

        _source.Stop();
        _source.loop = false;
        footstepLoopCoroutine = null;
    }
   void Awake()
{
   

    AudioSource[] sources = GetComponents<AudioSource>();
    _source = sources[0];     
    _oneShots = sources[1];   

    input = GetComponentInParent<PlayerInputController>();
    groundCheck = GetComponentInParent<GroundCheck>();
}



    void Start()
    {

        
        powerUp = Object.FindFirstObjectByType<PowerUpBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleSound();
    }

    public void PlaySound(AudioClip clip, float pitchMin = 0.8f, float pitchMax = 1.2f)
    {
        _source.clip = clip;
        _source.pitch = Random.Range(pitchMin, pitchMax);
        _source.Play();
    }



    void HandleSound()
    {
      


        if (footstepLoopCoroutine != null && (!groundCheck.isGrounded || input.jumping))
        {
            StopCoroutine(footstepLoopCoroutine);
            _source.Stop();
            _source.loop = false;
            footstepLoopCoroutine = null;
        }


        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && footstepLoopCoroutine == null && groundCheck.isGrounded)
        {
            footstepLoopCoroutine = StartCoroutine(FootstepLoop(_playerRun));
        }

        // Jump sound
        if ((input.jumping || Input.GetKeyDown(KeyCode.W)) && !hasPlayedJumpSound)
        {
            _oneShots.PlayOneShot(_playerJump);
            hasPlayedJumpSound = true;
        }
        if (!input.jumping)
        {
            hasPlayedJumpSound = false;
        }

        if (powerUp.IsTaken)
            _oneShots.PlayOneShot(_coockiePowerUp);

    
   
}
}

