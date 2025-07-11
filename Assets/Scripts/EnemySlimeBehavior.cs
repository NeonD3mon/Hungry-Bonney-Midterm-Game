using System.Collections;
using UnityEngine;

public class EnemySlimeBehavior : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    private Utilities utilities;
    public BoxCollider2D wallCheckRight;
    public BoxCollider2D wallCheckLeft;
    public EdgeCollider2D slimeBodyCollider;
    public LayerMask wallMask;
    private Vector2 SlimeMovement;
    public bool jumping;
    public int health = 2;
    public bool isHit = false;

    public bool isAlive = true;
    bool GoRight;

    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _horizontalMove = 3.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>();


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
                rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                // Go left if not at wall
                if (!IsTouchingWall(Vector2.left))
                    SlimeMovement.x = -_horizontalMove;
                else
                    SlimeMovement.x = _horizontalMove;

                yield return new WaitForSeconds(0.5f);
                rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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

    public void TakeDamage(int damage)
    {
        isHit = true;
        animator.SetTrigger("isHit");
        health -= damage;
        Debug.Log($"Enemy health: {health}");
        isHit = false;
        if (health <= 0)
        {
            isAlive = false;
           
            Destroy(gameObject, 2f);
        }
        animator.SetBool("isAlive", isAlive);
    }
   
    

}






