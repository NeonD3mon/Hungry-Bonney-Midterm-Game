using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private Camera mainCam;
    private PlayerAnimation playerAnimation;
    private GunRecoil gunRecoil;
    private GroundCheck groundCheck;
    public bool jumping { get; set; }
    public bool attacking { get; set; }
    public Vector2 direction { get; set; }
    private Vector2 lastValidDirection = Vector2.right;
    private float lastAttackTime;
    public float MoveDirection { get; set; }

    

    [SerializeField] float attackCooldown = 0.3f;
    [SerializeField] KeyCode _attack;
    [SerializeField] KeyCode _jump;



    void Start()
    {
        mainCam = Camera.main;
        playerAnimation = GetComponent<PlayerAnimation>();
        groundCheck = GetComponent<GroundCheck>();
        gunRecoil = GetComponent<GunRecoil>();
    
    }

    
    void Update()
    {
        HandleInput();
        UpdateDirection();

    }

    void HandleInput()
    {
        MoveDirection = Input.GetAxis("Horizontal");
        if (groundCheck.isGrounded && Input.GetKeyDown(_jump))
            jumping = true;


        if (Input.GetKeyDown(_attack) && Time.time >= lastAttackTime + attackCooldown)
        {
            UpdateDirection();
            PlayerAttack();
            gunRecoil.ApplyRecoil(lastValidDirection);
    
            lastAttackTime = Time.time;
        }
    }

    public void UpdateDirection()
    {
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rawDir = mouseWorldPos - transform.position;

        // Only update if the direction is long enough
        if (rawDir.magnitude > 0.5f)
        {
            direction = rawDir.normalized;
            lastValidDirection = direction;
        }
        else
        {
            // Keep using the last valid aim direction
            direction = lastValidDirection;
        }
        Debug.Log($"Shooting with lastValidDirection: {lastValidDirection}");
}

    void PlayerAttack()
    {
        attacking = true;
        playerAnimation.animator.SetTrigger("attack");
    }



}
