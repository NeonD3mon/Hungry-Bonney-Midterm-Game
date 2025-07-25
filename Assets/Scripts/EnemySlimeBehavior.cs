using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemySlimeBehavior : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    private Utilities utilities;
    private SoundManagerScript soundManager;
    public BoxCollider2D wallCheckRight;
    public BoxCollider2D wallCheckLeft;
    public EdgeCollider2D slimeBodyCollider;
    private Light2D innerLight;
    public LayerMask wallMask;
    private Vector2 SlimeMovement;
    public bool jumping;
    public int health = 2;
    public bool isHit = false;

    public bool isAlive = true;
    bool GoRight;

    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _horizontalMove = 3.5f;
    private bool isVisible = false;
    private AudioSource _source;
    public AudioClip _slimeDamage;
    public AudioClip _slimeDie;
    public AudioClip _slimeJump;


    void OnBecameVisible()
    {
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        isVisible = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>();
        innerLight = GetComponentInChildren<Light2D>();
        _source = GetComponent<AudioSource>();


        Debug.Log($"rb: {rb}");
        Debug.Log($"animator: {animator}");
        Debug.Log($"groundCheck: {groundCheck}");


        isAlive = true;
        health = 2;
        StartCoroutine(Movement());
    }


    void Update()
    {
        jumping = !groundCheck.isGrounded;
        EnemySlimeAnimate();
        HandleSound();
    }

    IEnumerator Movement()
    {
        while (isAlive)
        {
            int choice = Random.Range(0, 3);

            if (choice == 0)
            {
                // Stay in place
                SlimeMovement.x = 0;
            }
            else if (choice == 1)
            {
                // Go right if not at wall
                if (!IsTouchingWall(Vector2.right))
                    SlimeMovement.x = _horizontalMove;
                else
                    SlimeMovement.x = -_horizontalMove;

                yield return new WaitForSeconds(0.5f);
                jumping = true;
                rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                jumping = false;
            }
            else
            {
                // Go left if not at wall
                if (!IsTouchingWall(Vector2.left))
                    SlimeMovement.x = -_horizontalMove;
                else
                    SlimeMovement.x = _horizontalMove;

                yield return new WaitForSeconds(0.5f);
                jumping = true;
                rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                jumping = false;

            }


            rb.linearVelocity = new Vector2(SlimeMovement.x, rb.linearVelocity.y);

            yield return new WaitForSeconds(3f);
        }
    }

    bool IsTouchingWall(Vector2 direction)
    {
        float distance = 0.1f;
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, wallMask);
        return hit.collider != null;
    }

    void EnemySlimeAnimate()
    {
        float EnemyDirection = Mathf.Sign(SlimeMovement.x);
        transform.localScale = new Vector3(EnemyDirection, 1, 1);
        animator.SetBool("jump", jumping);
        animator.SetBool("isGrounded", groundCheck.isGrounded);


    }

    void HandleSound()
    {
        if (isVisible == true && jumping == true)
            _source.PlayOneShot(_slimeJump);

       // if (isVisible == true && isAlive == false)
        //    _source.PlayOneShot(_slimeDie);
    }

    public void TakeDamage(int damage)
    {
        
        animator.SetTrigger("isHit");
        health -= damage;
        Debug.Log($"Enemy health: {health}");



        if (health <= 0)
        {
            isAlive = false;
            if (isVisible && _slimeDie != null)
                _source.PlayOneShot(_slimeDie);

            StartCoroutine(DisableLightAfterDelay(0.5f));
            Destroy(gameObject, 2f);
        }
        else if (isVisible && _slimeDamage != null)
        {
            Debug.Log($"Trying to play slime damage: {_slimeDamage.name}, health: {health}");
            _source.PlayOneShot(_slimeDamage);
        }
        

        animator.SetBool("isAlive", isAlive);
    }
    IEnumerator DisableLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(innerLight);
    }

   
    
 
}






