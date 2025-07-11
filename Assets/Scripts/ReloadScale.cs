using UnityEngine;

public class ReloadScale : MonoBehaviour
{
    private Vector3 initialLocalScale;
    private PlayerAnimation animator;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite flippedSprite;
    [SerializeField] private SpriteRenderer reloadSprite;

    void Awake()
    {

        initialLocalScale = transform.localScale;
        animator = GetComponentInParent<PlayerAnimation>(); 
        reloadSprite = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() 
    {

        if (transform.parent != null)
        {
            if (animator.invertReladMessage == true)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                reloadSprite.sprite = flippedSprite;

            }
                
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                reloadSprite.sprite = normalSprite;
            }
               

        } 
    }
    
}
