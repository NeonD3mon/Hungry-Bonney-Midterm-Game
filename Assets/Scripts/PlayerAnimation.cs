using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private PlayerInputController input;
    private PlayerMovement playerMovement;
    private GroundCheck groundCheck;
    public Animator animator { get; set; }
    float playerDirection = 1f;

    [SerializeField] private Transform armTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInputController>();
        groundCheck = GetComponent<GroundCheck>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimate();
    }
    
    void PlayerAnimate()
    {

        // if (Mathf.Abs(MoveDirection) > 0.01f)


        

        if (Mathf.Abs(input.direction.x) > 0.1f)
        {
            float angle = Mathf.Atan2(input.direction.y, input.direction.x) * Mathf.Rad2Deg;
            armTransform.rotation = Quaternion.Euler(0, 0, angle);
            if (input.direction.x < 0)
                armTransform.localScale = new Vector3(-1, -1, 1); // Flip the gunwith the player


            else
                armTransform.localScale = new Vector3(1, 1, 1);

            playerDirection = Mathf.Sign(input.direction.x);
            transform.localScale = new Vector3(playerDirection, 1, 1);
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
