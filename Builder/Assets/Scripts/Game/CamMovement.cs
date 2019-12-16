using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [Header("Rotation")]
    public float RotationSpeedX = 2.0f;
    public float RotationSpeedY = 2.0f;

    [Header("Movement")]
    public bool panCamera = true;
    public float panSpeed = 20.0f;
    public float panBorderThickness = 10.0f;
    public float scrollSpeed = 20.0f;

    //TODO: Make this based on the size of the map. NOT hardcode it into the inspector
    public Vector2 panLimit = new Vector2(5.0f, 5.0f);

    private float _x = 0.0f;
    private float _y = 0.0f;

    private const float _rotateMinX = 30.0f;
    private const float _rotateMaxX = 70.0f;
    private const float _scrollMinY = 4.0f;
    private const float _scrollMaxY = 70.0f;


    void Start()
    {
        // Initial X and Y values of the camera rotation
        _x = transform.eulerAngles.x;
        _y = transform.eulerAngles.y;
    }
    
    void Update()
    {
        #region CameraRotation

        // Change the camera rotation when the left mouse button is clicked
        if (Input.GetMouseButton(1))
        {
            // Get the mouse position
            _y += RotationSpeedX * Input.GetAxis("Mouse X");
            _x -= RotationSpeedY * Input.GetAxis("Mouse Y");

            // Limit X rotation to a minimum and maximum value
            _x = Mathf.Clamp(_x, _rotateMinX, _rotateMaxX);

            // Change rotation
            transform.eulerAngles = new Vector3(_x, _y, 0.0f);
        }

        #endregion

        #region CameraMovement

        // Gets the initial position
        Vector3 pos = transform.position;

        // Disable panning if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panCamera = !panCamera;

        }
        if (panCamera)
        {
            // W key or Upper border
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            // S key or Bottom border
            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            // D key or Right border
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            // A key or Left border
            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
        }

        // Gets the ScrollWheel Input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;

        // Limits the camera movement to be on the map
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, _scrollMinY, _scrollMaxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Move the Camera to new position
        transform.position = pos;

        #endregion
    }
}
