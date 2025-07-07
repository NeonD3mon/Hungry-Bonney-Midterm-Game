using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(4f, 0f, -10f);
    void Start()
    {
        
    }

   
    void Update()
    {
       /* transform.position = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            offset.z); */
    }
}
