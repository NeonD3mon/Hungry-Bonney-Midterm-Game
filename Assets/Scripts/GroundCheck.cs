using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded { set; get; }
    public LayerMask groundMask;
    public BoxCollider2D groundCheck;

    void Start()
    {
        isGrounded = true;

    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
    }
    
    public void CheckGround()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }
}
