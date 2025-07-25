using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private PlayerInputController input;
    private GameBehavior game;
    private GroundCheck groundCheck;
    public BoxCollider2D wallCheck;
    public LayerMask wallMask;
    private GunBehavior gun;
    private const float RecoilThreshold = 1e-5f;
    private bool isTouchingWall = false;
    bool cancelRecoilDelay = true;
    public float MovementSpeed = 4f;
    public float JumpForce = 5.0f;
    public float GroundDecay = 4.2f;

    public float recoilDecay = 0.8f;

  




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputController>();
        game = GameBehavior.Instance;
        gun = GetComponent<GunBehavior>();
        groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        MovePlayer();
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            Utilities.DamagePlayer(other.gameObject, 1);
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & wallMask) != 0)
        {
            isTouchingWall = true;
            cancelRecoilDelay = true;
            input.MoveDirection = 0;
            //Debug.Log("Touching wall");
        }

        if (other.gameObject.CompareTag("killZone"))
            GameBehavior.Instance.RestartLevel();

        if (other.gameObject.CompareTag("winZone"))
        {
            game.StopTimer();
            SceneManager.LoadScene(2);
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (((1 << other.gameObject.layer) & wallMask) == 0)
        {
            isTouchingWall = false;
            //Debug.Log("Not Touching wall");
        }

    }


    void MovePlayer()
{
    float dt = Time.fixedDeltaTime;

    Vector2 velocity = rb.linearVelocity;
    Vector2 recoilDelta = gun.GetRecoilDelta(dt);

    // Jump
    if (input.jumping)
    {
        velocity.y = JumpForce;
        input.jumping = false;
    }
    else
    {
       
        velocity.y += gun.GetRecoilDelta(dt).y;
    }

    bool grounded = groundCheck.isGrounded;
    velocity.y += recoilDelta.y;
    if (grounded || isTouchingWall)
    {
        // Ground or wall horizontal movement
        if (Mathf.Abs(input.MoveDirection) > 0.01f)
        {
            velocity.x = input.MoveDirection * MovementSpeed;
        }
        else
        {
            // Strong friction if standing still
            //velocity.x = Mathf.MoveTowards(velocity.x, 0f, 30f * dt);
        }

            if (gun.recoilVelocity.y > 0.1f)
            {
                // Adjust the horizontal recoil strength as you like (e.g. 0.3f)
                float horizontalNudge = recoilDelta.x * 0.5f;
                velocity.x += horizontalNudge;
            }

       

        velocity.x += recoilDelta.x / 2f;

            // Optional: simulate a "hop" if recoil is strong

            if (recoilDelta.y > 0.3f)
            {
                velocity.y = recoilDelta.y;
            }
    }
    else
    {
        // Air control
        float airControl = 12f;
        velocity.x = Mathf.MoveTowards(velocity.x, input.MoveDirection * MovementSpeed, airControl * dt);

        // Full recoil in air
        velocity += recoilDelta;
    }


        if (gun.recoilVelocity.magnitude < 0.1f)
        {
            velocity.x = Mathf.Clamp(velocity.x, -10f, 10f);
            velocity.y = Mathf.Clamp(velocity.y, -20f, 20f);
        }
    rb.linearVelocity = velocity;

    //Debug.Log($"Assigned Velocity: {velocity}, Recoil: {gunRecoil.recoilVelocity}, MoveDir: {input.MoveDirection}");
}




}
