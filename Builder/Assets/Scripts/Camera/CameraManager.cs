using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Positioning")]
    public Vector2 CameraOffset = new Vector2(10f, 14f);
    public float lookAtOffset = 2f;

    [Header("Move Controls")]
    public float InOutSpeed = 5f;
    public float LateralSpeed = 5f;
    public float RotateSpeed = 45f;

    [Header("Movement Bounds")]
    public Vector2 MinBounds;
    public Vector2 MaxBounds;

    [Header("Zoom Controls")]
    public float ZoomSpeed = 4f;
    public float NearZoomLimit = 2f;
    public float FarZoomLimit = 16f;
    public float StartingZoom = 5f;

    private IZoomStrategy _zoomStrategy;
    private Vector3 _frameMove;
    private float _frameRotate;
    private float _frameZoom;
    private Camera _cam;


    private void Awake()
    {
        // Gets the camera from the child object
        _cam = GetComponentInChildren<Camera>();


        _cam.transform.localPosition = new Vector3(0f, Mathf.Abs(CameraOffset.y), -Mathf.Abs(CameraOffset.x));

        // Use a different strategy based on the camera's projection (Set in the Inspector)
        _zoomStrategy = _cam.orthographic ? (IZoomStrategy) new OrthographicZoomStrategy(_cam, StartingZoom) 
            : new PerspectiveZoomStrategy(_cam, CameraOffset, StartingZoom);

        // Look at the empty gameobject
        _cam.transform.LookAt(transform.position + Vector3.up * lookAtOffset);
    }

    private void OnEnable()
    {
        // Keyboard Input
        KeyboardInputManager.OnMoveInput += UpdateFrameMove;
        KeyboardInputManager.OnRotateInput += UpdateFrameRotate;
        KeyboardInputManager.OnZoomInput += UpdateFrameZoom;

        // Mouse Input
        MouseInputManager.OnMoveInput += UpdateFrameMove;
        MouseInputManager.OnRotateInput += UpdateFrameRotate;
        MouseInputManager.OnZoomInput += UpdateFrameZoom;
    }

    private void OnDisable()
    {
        // Keyboard Input
        KeyboardInputManager.OnMoveInput -= UpdateFrameMove;
        KeyboardInputManager.OnRotateInput -= UpdateFrameRotate;
        KeyboardInputManager.OnZoomInput -= UpdateFrameZoom;

        // Mouse Input
        MouseInputManager.OnMoveInput -= UpdateFrameMove;
        MouseInputManager.OnRotateInput -= UpdateFrameRotate;
        MouseInputManager.OnZoomInput -= UpdateFrameZoom;
    }

    private void UpdateFrameMove(Vector3 moveVector)
    {
        _frameMove += moveVector;
    }

    private void UpdateFrameRotate(float rotateAmount)
    {
        _frameRotate += rotateAmount;
    }

    private void UpdateFrameZoom(float zoomAmount)
    {
        _frameZoom += zoomAmount;
    }

    private void LateUpdate()
    {
        // Movement
        if (_frameMove != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(_frameMove.x * LateralSpeed, _frameMove.y, _frameMove.z * InOutSpeed);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;
            LockPositionInBounds();
            _frameMove = Vector3.zero;
        }

        // Rotation
        if (_frameRotate != 0)
        {
            transform.Rotate(Vector3.up, _frameRotate * Time.deltaTime * RotateSpeed);
            _frameRotate = 0f;
        }

        // Zoom
        if (_frameZoom < 0f)
        {
            _zoomStrategy.ZoomIn(_cam, Time.deltaTime * Mathf.Abs(_frameZoom) * ZoomSpeed, NearZoomLimit);
            _frameZoom = 0f;
        }
        else if (_frameZoom > 0f)
        {
            _zoomStrategy.ZoomOut(_cam, Time.deltaTime * _frameZoom * ZoomSpeed, FarZoomLimit);
            _frameZoom = 0f;
        }
    }

    private void LockPositionInBounds()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, MinBounds.x, MaxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, MinBounds.y, MaxBounds.y)
            );
    }
}
