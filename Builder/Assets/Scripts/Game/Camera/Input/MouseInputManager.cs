using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : InputManager
{
    private Vector2Int _screen;
    private float _mousePositionOnRotateStart;

    // Events
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;
    public static event ZoomInputHandler OnZoomInput;

    private void Awake()
    {
        // Get the screen height and width
        _screen = new Vector2Int(Screen.width, Screen.height);
    }

    private void Update()
    {
        // Get the mouse position
        Vector3 mp = Input.mousePosition;

        // The mouse movement will be valid only within 5% of the screen's width and height
        bool mouseValid = (mp.y <= _screen.y * 1.05f && mp.y >= _screen.y * -0.05f && 
            mp.x <= _screen.x * 1.05f && mp.x >= _screen.x * -0.05f);
        if (!mouseValid)
            return;

        // Movement
        if (mp.y > _screen.y * 0.95f)
        {
            OnMoveInput?.Invoke(Vector3.forward);
        }
        else if (mp.y < _screen.y * 0.05f)
        {
            OnMoveInput?.Invoke(-Vector3.forward);
        }
        if (mp.x > _screen.x * 0.95f)
        {
            OnMoveInput?.Invoke(Vector3.right);
        }
        else if (mp.x < _screen.x * 0.05f)
        {
            OnMoveInput?.Invoke(-Vector3.right);
        }

        // Rotation
        if (Input.GetMouseButtonDown(1))
        {
            _mousePositionOnRotateStart = mp.x;
        }
        else if (Input.GetMouseButton(1))
        {
            if (mp.x < _mousePositionOnRotateStart)
            {
                OnRotateInput?.Invoke(-1f);
            }
            else if (mp.x > _mousePositionOnRotateStart)
            {
                OnRotateInput?.Invoke(1f);
            }
        }

        // Zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            OnZoomInput?.Invoke(-3f);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            OnZoomInput?.Invoke(3f);
        }
    }

}
