using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GunBehavior gun;
    private bool hit;
    private float bulletSpeed;
    private Rigidbody2D rb;
    private Utilities utilities;
    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
       
        rb.gravityScale = 0f; // bullets should not fall
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true; // optional, keeps it from spinning
        Destroy(gameObject, 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetSpeed(Vector2 velocity)

    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        Debug.Log("Setting bullet speed to: " + velocity);
        rb.linearVelocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            hit = true;
            Utilities.DamageEnemy(collision.gameObject, 1);
            Destroy(gameObject); // Destroy on hit
        }
        else if (collision.CompareTag("Platform"))
            Destroy(gameObject); // Destroy on hit
        
    }
    // Destroy the bullet after 2 seconds to prevent memory leaks
}
