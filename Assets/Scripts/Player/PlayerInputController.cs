
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private Camera mainCam;
    private PlayerAnimation playerAnimation;
    private GunBehavior gun;
    private GroundCheck groundCheck;
    private SoundManagerScript soundManager;
    private Bullet bullet;
    public bool jumping { get; set; }
    public bool attacking { get; set; }
    public bool moveCamera { get; set; }
    public Vector2 direction { get; set; }
    private Vector2 lastValidDirection = Vector2.right;
    private float lastAttackTime;
    public float MoveDirection { get; set; }
    public Utilities.GameState CurrentState;

    

    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] KeyCode _attack;
    [SerializeField] KeyCode _jump;
    [SerializeField] KeyCode _reload;



    void Start()
    {
        CurrentState = Utilities.GameState.Play;
        mainCam = Camera.main;
        playerAnimation = GetComponent<PlayerAnimation>();
        groundCheck = GetComponent<GroundCheck>();
        gun = GetComponent<GunBehavior>();
        bullet = GetComponent<Bullet>();
    
    }


    void Update()
    {
        if (CurrentState == Utilities.GameState.Play)
        {
            HandleInput();
            UpdateDirection();
        }
        else
            return;
       

    }

    void HandleInput()
    {
       
        
            MoveDirection = Input.GetAxis("Horizontal");
            
            if (groundCheck.isGrounded && Input.GetKeyDown(_jump) || groundCheck.isGrounded && Input.GetKeyDown(KeyCode.W))
            jumping = true;


            if (Input.GetKeyDown(_attack) && Time.time >= lastAttackTime + attackCooldown && gun.reloading == false)
            {
                UpdateDirection();
                attacking = true;
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
        
        moveCamera = true;
        gun.Fire();
        gun.PlaySound(gun._fire);
        playerAnimation.animator.SetTrigger("attack");
        attacking = false;
    }


}
