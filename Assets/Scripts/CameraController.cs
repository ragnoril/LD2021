using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MinimumX;
    public float MaximumX;

    public float MinimumY;
    public float MaximumY;

    public float MinimumZ;
    public float MaximumZ;

    public float ScreenDelta;

    public float CameraMoveSpeed = 1.0f;
    public float CameraZoomSpeed = 1.0f;

    void Update()
    {
        if (Input.GetMouseButton(1))
            GetUserInput(Time.deltaTime);

    }

    void GetUserInput(float dt)
    {
        Vector3 translation = Vector3.zero;

        if (Input.mousePosition.x >= Screen.width - ScreenDelta)
        {
            // Move the camera
            translation += transform.right * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.x <= 0 + ScreenDelta)
        {
            // Move the camera
            translation -= transform.right * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.y >= Screen.height - ScreenDelta)
        {
            // Move the camera
            translation += transform.up * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.y <= 0 + ScreenDelta)
        {
            // Move the camera
            translation -= transform.up * CameraMoveSpeed * dt;
        }


        // Zoom in or out
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * CameraZoomSpeed * dt;
        if (zoomDelta != 0)
        {
            translation += transform.forward * zoomDelta;
        }
        transform.position += translation;

        translation = transform.position;

        translation.x = Mathf.Clamp(translation.x, MinimumX, MaximumX);
        translation.y = Mathf.Clamp(translation.y, MinimumY, MaximumY);
        translation.z = Mathf.Clamp(translation.z, MinimumZ, MaximumZ);

        transform.position = translation;

    }
}
