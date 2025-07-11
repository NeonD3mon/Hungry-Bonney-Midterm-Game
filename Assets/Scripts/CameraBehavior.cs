using System;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform player;
    private PlayerInputController input;
    public Vector3 offset = new Vector3(3f, 1f, -10f);

    [SerializeField] float CameraZoomMin = 9f;
    [SerializeField] float CameraZoomMax = 13;

    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        input = GetComponent<PlayerInputController>();
    }


    void Update()
    {
        CameraMovement();
        CameraDistance();
    }

    void CameraMovement()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            offset.z);
    }

    void CameraDistance()
    {
        float cameraZoom = player.position.y;
        
        
            mainCamera.orthographicSize = Mathf.Clamp(cameraZoom + 4f, CameraZoomMin, CameraZoomMax) / 2;
            //Debug.Log($"cameraZoom: {cameraZoom}");
        
        
    }
}
