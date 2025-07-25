using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private PlayerInputController input;
    private PlayerMovement playerMovement;
    private GroundCheck groundCheck;
    private GunBehavior gun;
    public Animator animator { get; set; }
    public GameObject reloadingMassage;
    float playerDirection = 1f;
    public bool invertReladMessage { get; set; }

    [SerializeField] private Transform armTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInputController>();
        groundCheck = GetComponent<GroundCheck>();
        playerMovement = GetComponent<PlayerMovement>();
        gun = GetComponent<GunBehavior>();
        reloadingMassage = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimate();
    }

    void PlayerAnimate()
    {

        




        if (Mathf.Abs(input.direction.x) > 0.1f)
        {
            float angle = Mathf.Atan2(input.direction.y, input.direction.x) * Mathf.Rad2Deg;
            armTransform.rotation = Quaternion.Euler(0, 0, angle);
            if (input.direction.x < 0)
                armTransform.localScale = new Vector3(-1, -1, 1); // Flip the gun with the player


            else
                armTransform.localScale = new Vector3(1, 1, 1);

            playerDirection = Mathf.Sign(input.direction.x);
            transform.localScale = new Vector3(playerDirection, 1, 1);
            if (playerDirection < 0)
                invertReladMessage = true;
            else
                invertReladMessage = false;
            
        }
        else
            return;



        //Animation Setting
        animator.SetBool("run", input.MoveDirection != 0);
        animator.SetBool("isGrounded", groundCheck.isGrounded);
        animator.SetFloat("velocityY", playerMovement.rb.linearVelocity.y);
        animator.SetBool("jump", input.jumping);
        

    }
}
