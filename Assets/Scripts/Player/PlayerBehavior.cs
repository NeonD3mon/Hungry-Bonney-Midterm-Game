
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCam;
    private Animator animator;
    public LayerMask groundMask;
    public BoxCollider2D groundCheck;
    public BoxCollider2D wallCheck;
    public LayerMask wallMask;
    public float MovementSpeed = 4f;
    public float JumpForce = 5.0f;
    //public float AirInfluence = 0.5f;
    private float MoveDirection;
    private Vector2 recoilVelocity = Vector2.zero;
    public float recoilDecaySpeed = 5f;  // Tune this value for smooth decay


    private bool isTouchingWall = false;
    bool cancelRecoilDelay = true;

    [SerializeField] float recoilAmount = 2f;
    [SerializeField] private Transform armTransform;
    [SerializeField] float attackCooldown = 0.3f;

    private float lastAttackTime;


    Vector2 direction;

    Vector3 rotation;

    float playerDirection = 1f;

    public float GroundDecay = 4.2f;

    public bool isGrounded;

    private bool jumping;

    private bool attacking;




    public float drag = 0.1f;



    [SerializeField] KeyCode _attack;

    [SerializeField] KeyCode _jump;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateDirection();
        PlayerAnimate();






       



    }

    void FixedUpdate()
    {
        PlayerMovement();
        GroundCheck();






    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }*/

        if (((1 << other.gameObject.layer) & wallMask) != 0)
        {
            isTouchingWall = true;
            cancelRecoilDelay = true;
            MoveDirection = 0;
            Debug.Log("Touching wall");
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /* if (other.gameObject.CompareTag("Platform"))
         {
             isGrounded = false;
             isTouchingWall = false;
             Debug.Log("Not Grounded");
         } */

        if (((1 << other.gameObject.layer) & wallMask) == 0)
        {
            isTouchingWall = false;
            Debug.Log("Not Touching wall");
        }

    }

    void PlayerMovement()
    {
        Vector2 velocity = rb.linearVelocity;

        if (jumping)
        {
            velocity.y = JumpForce;
            jumping = false;

        }

        // Movement input overrides only if recoil velocity is small
        if (recoilVelocity.magnitude < 0.00000001f || isTouchingWall || isGrounded)
        {


            if (Mathf.Abs(MoveDirection) > 0.01f)
                velocity.x = MoveDirection * MovementSpeed;
            else
            {
                velocity.x *= GroundDecay;
            }


        }
        else if (Mathf.Abs(MoveDirection) > 0.1f && Mathf.Sign(direction.x) != Mathf.Sign(MoveDirection))
            velocity.x = MoveDirection * MovementSpeed;

        else if (Mathf.Abs(MoveDirection) > 0.1f && Mathf.Sign(direction.x) == Mathf.Sign(MoveDirection))
            velocity.x = Mathf.Min(MoveDirection * MovementSpeed, velocity.x);

        velocity += recoilVelocity;

        float dumpingFactor = 0.8f;

        recoilVelocity *= dumpingFactor;

        Vector2 deltaV = velocity - rb.linearVelocity;
        Debug.Log($"Frame DeltaV: {deltaV}");

        rb.linearVelocity = velocity;

        if (attacking)
        {
            Vector2 rawDir = -direction.normalized;
            rawDir.y = Mathf.Max(rawDir.y, 0f);

            Vector2 recoilDir = new Vector2(rawDir.x * 0.7f, rawDir.y * 1.7f).normalized;
            recoilVelocity = recoilDir * recoilAmount;
            attacking = false;
        }
        //Debug.Log($"Velocity: {rb.linearVelocity}, Recoil: {recoilVelocity}");
    }
    void PlayerAnimate()
    {

        // if (Mathf.Abs(MoveDirection) > 0.01f)


        //Gun rotation
        UpdateDirection();
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            armTransform.rotation = Quaternion.Euler(0, 0, angle);
            if (direction.x < 0)
                armTransform.localScale = new Vector3(-1, -1, 1); // Flip the gunwith the player


            else
                armTransform.localScale = new Vector3(1, 1, 1);

            playerDirection = Mathf.Sign(direction.x);
            transform.localScale = new Vector3(playerDirection, 1, 1);
        }
        else
            return;



        //Animation Setting
        animator.SetBool("run", MoveDirection != 0);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("velocityY", rb.linearVelocity.y);
        animator.SetBool("jump", jumping);

    }

    void PlayerAttack()
    {
        attacking = true;
        animator.SetTrigger("attack");
    }

    void PlayerJump()
    {
        
    }

    void HandleInput()
    {
        MoveDirection = Input.GetAxis("Horizontal");
         if (isGrounded && Input.GetKeyDown(_jump))
            jumping = true;


        if (Input.GetKeyDown(_attack) && Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerAttack();
            lastAttackTime = Time.time;
        }
    }


    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }
    
    void UpdateDirection()
{
    Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    direction = (mouseWorldPos - transform.position).normalized;
}
} 
    

   

