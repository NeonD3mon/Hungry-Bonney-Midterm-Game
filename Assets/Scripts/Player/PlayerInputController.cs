using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private Camera mainCam;
    private PlayerAnimation playerAnimation;
    private GunBehavior gun;
    private GroundCheck groundCheck;
    private Bullet bullet;
    public bool jumping { get; set; }
    public bool attacking { get; set; }
    public bool moveCamera { get; set; }
    public Vector2 direction { get; set; }
    private Vector2 lastValidDirection = Vector2.right;
    private float lastAttackTime;
    public float MoveDirection { get; set; }

    

    [SerializeField] float attackCooldown = 0.3f;
    [SerializeField] KeyCode _attack;
    [SerializeField] KeyCode _jump;
    [SerializeField] KeyCode _reload;



    void Start()
    {
        mainCam = Camera.main;
        playerAnimation = GetComponent<PlayerAnimation>();
        groundCheck = GetComponent<GroundCheck>();
        gun = GetComponent<GunBehavior>();
        bullet = GetComponent<Bullet>();
    
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


        if (Input.GetKeyDown(_attack) && Time.time >= lastAttackTime + attackCooldown && gun.reloading == false)
        {
            UpdateDirection();
            PlayerAttack();
            gun.ApplyRecoil(lastValidDirection);

            lastAttackTime = Time.time;
        }
        if (Input.GetKeyDown(_reload) && gun.Magazine > 0)
        {
            gun.reloading = true;
            gun.StartCoroutine(gun.Reload());
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
        //Debug.Log($"Shooting with lastValidDirection: {lastValidDirection}");
}

    void PlayerAttack()
    {  
        attacking = true;
        moveCamera = true;
        gun.Fire();
        playerAnimation.animator.SetTrigger("attack");
    }


}
