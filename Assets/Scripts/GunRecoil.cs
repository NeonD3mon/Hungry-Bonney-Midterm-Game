using UnityEngine;
using UnityEngine.InputSystem;

public class GunRecoil : MonoBehaviour
{
    private PlayerInputController input;
    public Vector2 recoilVelocity { get; set; } = Vector2.zero;
    public float recoilDamping = 5f;

    [SerializeField] float recoilAmount = 5f;
    
    [SerializeField] float recoilXMultiplier = 2f; 
    [SerializeField] float recoilYMultiplier = 0.5f; 


    void Start()
    {
        input = GetComponent<PlayerInputController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void ApplyRecoil(Vector2 direction)
{
    if (direction.magnitude < 0.1f)
    {
        Debug.LogWarning("ApplyRecoil called with near-zero direction. Recoil not applied.");
        return;
    }

    direction.Normalize(); // make sure it's a unit vector

    Vector2 scaledDir = new Vector2(
        direction.x * recoilXMultiplier,
        direction.y * recoilYMultiplier // always push up
    );

    recoilVelocity = -scaledDir * recoilAmount;

    Debug.Log($"Applied Recoil: {recoilVelocity}");
}


    public Vector2 GetRecoilDelta(float deltaTime)
    {
        
    // Calculate how much recoil velocity is applied this frame
        Vector2 delta = recoilVelocity * deltaTime * 10f;

    // Dampen recoil velocity for next frame
        recoilVelocity *= Mathf.Exp(-recoilDamping * deltaTime);
        
        
        return delta;


       
        
    }


    /*public void ApplyRecoil()
    {
        if (!input.attacking)
             //return;

         Vector2 aimDir = input.direction;
         //if (input.attacking)
         //{
              // Don't apply recoil if direction is invalid
         if (aimDir.magnitude < 0.1f)
             return;

         Vector2 rawDir = -aimDir;

         // Avoid downward recoil
         rawDir.y = Mathf.Clamp(rawDir.y, 0f, 1f);

         Vector2 recoilDir = rawDir.normalized;
         recoilDir.y = Mathf.Clamp(recoilDir.y, 0.1f, 0.7f);
         recoilDir = recoilDir.normalized;

         Debug.Log($"Aim direction: {aimDir}, recoilDir: {recoilDir}, recoilVelocity: {recoilVelocity}");


         Vector2 recoil = recoilDir * recoilAmount;
         float maxRecoil = 4f;
         if (recoil.magnitude > maxRecoil)
         recoil = recoil.normalized * maxRecoil;

         recoilVelocity += recoil;
         if (recoilVelocity.magnitude > maxRecoil)
             recoilVelocity = recoilVelocity.normalized * maxRecoil;


         recoilVelocity = Vector2.ClampMagnitude(recoilVelocity, maxRecoil);
         input.attacking = false;
         //}


         Debug.Log($"Applied Recoil: {recoilVelocity}");

        

    }*/
}
