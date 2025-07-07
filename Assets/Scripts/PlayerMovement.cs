using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    private PlayerInputController input;
    private GroundCheck groundCheck;
    public BoxCollider2D wallCheck;
    public LayerMask wallMask;
    private GunRecoil gunRecoil;
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
        gunRecoil = GetComponent<GunRecoil>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & wallMask) != 0)
        {
            isTouchingWall = true;
            cancelRecoilDelay = true;
            input.MoveDirection = 0;
            Debug.Log("Touching wall");
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (((1 << other.gameObject.layer) & wallMask) == 0)
        {
            isTouchingWall = false;
            Debug.Log("Not Touching wall");
        }

    }


    void MovePlayer()
{
    float dt = Time.fixedDeltaTime;

    Vector2 velocity = rb.linearVelocity;
    Vector2 recoilDelta = gunRecoil.GetRecoilDelta(dt);

    // Jump
    if (input.jumping)
    {
        velocity.y = JumpForce;
        input.jumping = false;
    }
    else
    {
       
        velocity.y += gunRecoil.GetRecoilDelta(dt).y;
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

            if (gunRecoil.recoilVelocity.y > 0.1f)
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


        if (gunRecoil.recoilVelocity.magnitude < 0.1f)
        {
            velocity.x = Mathf.Clamp(velocity.x, -10f, 10f);
            velocity.y = Mathf.Clamp(velocity.y, -20f, 20f);
        }
    rb.linearVelocity = velocity;

    Debug.Log($"Assigned Velocity: {velocity}, Recoil: {gunRecoil.recoilVelocity}, MoveDir: {input.MoveDirection}");
}




}
