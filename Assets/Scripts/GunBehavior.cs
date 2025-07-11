using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehavior : MonoBehaviour
{
    private PlayerInputController input;
    private BoxCollider2D boxCollider;
    private GameBehavior game;
    public Vector2 recoilVelocity { get; set; } = Vector2.zero;
    public float recoilDamping = 5f;

    public bool reloading { get; set; } = false;
    public GameObject reloadingMessage;


    [SerializeField] float recoilAmount = 5f;
    [SerializeField] float recoilXMultiplier = 2f;
    [SerializeField] float recoilYMultiplier = 0.7f;
    public int Magazine { get; set; }
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;

    void Start()
    {
        reloadingMessage.SetActive(false);
        input = GetComponent<PlayerInputController>();
        boxCollider = GetComponent<BoxCollider2D>();
        game = GameBehavior.Instance;
        reloading = false;
        Magazine = 3;
        game.AmmoUI(Magazine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AmmoCheck()
    {
        if (reloading) return;

        else if (Magazine > 0)
        {
            Magazine -= 1;
            Debug.Log($"Ammo Count: {Magazine}");
            game.AmmoUI(Magazine);
        }

        if (Magazine == 0)
        {
            reloading = true;
            Debug.Log("reloading");
            StartCoroutine(Reload());
            

        }
    }
    public void ApplyRecoil(Vector2 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            //Debug.LogWarning("ApplyRecoil called with near-zero direction. Recoil not applied.");
            return;
        }

        direction.Normalize(); // make sure it's a unit vector

        Vector2 scaledDir = new Vector2(
            direction.x * recoilXMultiplier,
            direction.y * recoilYMultiplier // always push up
        );

        recoilVelocity = -scaledDir * recoilAmount;

        //Debug.Log($"Applied Recoil: {recoilVelocity}");
    }


    public Vector2 GetRecoilDelta(float deltaTime)
    {

        // Calculate how much recoil velocity is applied this frame
        Vector2 delta = recoilVelocity * deltaTime * 10f;

        // Dampen recoil velocity for next frame
        recoilVelocity *= Mathf.Exp(-recoilDamping * deltaTime);


        return delta;

    }

    public void Fire()
    {
       
        if (reloading || Magazine <= 0)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetSpeed(firePoint.right * bulletSpeed);
        AmmoCheck();
        
    }

    public IEnumerator Reload()
    {
        reloadingMessage.SetActive(true);
        yield return new WaitForSeconds(1f);
        Magazine = 3;
        reloading = false;
        reloadingMessage.SetActive(false);
        game.AmmoUI(Magazine);
        Debug.Log("Reloaded");
    }
    

    
}
