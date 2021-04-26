using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MinimumX;
    public float MaximumX;

    public float MinimumY;
    public float MaximumY;

    public float MinimumZ;
    public float MaximumZ;

    public float ScreenDeltaX;
    public float ScreenDeltaY;


    public float CameraMoveSpeed = 1.0f;
    public float CameraZoomSpeed = 1.0f;

    bool pan, goingRight, goingUp;
    Vector3 startPos, translation;
    private void Start()
    {
        //ResolutionAdjust();
    }
    void Update()
    {
        translation = Vector3.zero;
        if (Input.GetMouseButtonDown(1))
        {
            pan = true;
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1)) pan = false;

        //if (pan) translation = ScrollMouse(startPos, Time.deltaTime);
        if (!GameManager.instance.UI.PointerOverUI) translation = ScrollEdge(Time.deltaTime);
        translation = ScrollKeyboard(Time.deltaTime);
        translation = Zoom(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (MouseScreenCheck())
        {
            transform.position += translation;

            translation = transform.position;

            translation.x = Mathf.Clamp(translation.x, MinimumX, MaximumX);
            translation.y = Mathf.Clamp(translation.y, MinimumY, MaximumY);
            translation.z = Mathf.Clamp(translation.z, MinimumZ, MaximumZ);

            transform.position = translation;

        }
    }

    Vector3 ScrollKeyboard(float dt)
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            translation += transform.right * CameraMoveSpeed * dt;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            translation -= transform.right * CameraMoveSpeed * dt;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            translation += transform.up * CameraMoveSpeed * dt;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            translation -= transform.up * CameraMoveSpeed * dt;
        }
        return translation;
    }

    Vector3 ScrollMouse(Vector3 startPos, float dt)
    {
        var mousePosition = Input.mousePosition;
        if (mousePosition.x >= startPos.x)
        {
            translation += transform.right * CameraMoveSpeed * dt;
            goingRight = true;
        }
        if (mousePosition.x <= startPos.x)
        {
            // Move the camera
            translation -= transform.right * CameraMoveSpeed * dt;
            goingRight = false;
        }
        if (mousePosition.y >= startPos.y)
        {
            // Move the camera
            translation += transform.up * CameraMoveSpeed * dt;
            goingUp = true;
        }
        if (mousePosition.y <= startPos.y)
        {
            // Move the camera
            translation -= transform.up * CameraMoveSpeed * dt;
            goingUp = false;
        }


        return translation;
    }

    Vector3 ScrollEdge(float dt)
    {
        if (Input.mousePosition.x >= Screen.width - ScreenDeltaX)
        {
            // Move the camera
            translation += transform.right * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.x <= 0 + ScreenDeltaX)
        {
            // Move the camera
            translation -= transform.right * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.y >= Screen.height - ScreenDeltaY)
        {
            // Move the camera
            translation += transform.up * CameraMoveSpeed * dt;
        }
        if (Input.mousePosition.y <= 0 + ScreenDeltaY)
        {
            // Move the camera
            translation -= transform.up * CameraMoveSpeed * dt;
        }
        return translation;
    }

    Vector3 Zoom(float dt)
    {
        // Zoom in or out
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * CameraZoomSpeed * dt;
        if (zoomDelta != 0)
        {
            translation += transform.forward * zoomDelta;
        }
        return translation;
    }

    void ResolutionAdjust()
    {
        float height = Screen.width;
        float width = Screen.height;

        ScreenDeltaX = ScreenDeltaX * width / 100;
        ScreenDeltaY = ScreenDeltaY * height / 100;

    }
    bool MouseScreenCheck()
    {
#if UNITY_EDITOR
        if (Input.mousePosition.y <= 0 || Input.mousePosition.x <= 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
        {
            return false;
        }
#else
        if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }
#endif
        else
        {
            return true;
        }
    }


    }
