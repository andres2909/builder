using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveZoomStrategy : IZoomStrategy
{
    private Vector3 _normalizedCameraPosition;
    private float _currentZoomLevel;

    public PerspectiveZoomStrategy(Camera cam, Vector3 offset, float startingZoom)
    {
        _normalizedCameraPosition = new Vector3(0f, Mathf.Abs(offset.y), -Mathf.Abs(offset.x));
        _currentZoomLevel = startingZoom;
        PositionCamera(cam);
    }

    private void PositionCamera(Camera cam)
    {
        cam.transform.localPosition = _normalizedCameraPosition * _currentZoomLevel;
    }

    public void ZoomIn(Camera cam, float delta, float nearZoomLimit)
    {
        if (_currentZoomLevel <= nearZoomLimit)
            return;
        _currentZoomLevel = Mathf.Max(_currentZoomLevel - delta, nearZoomLimit);
        PositionCamera(cam);
    }

    public void ZoomOut(Camera cam, float delta, float farZoomLimit)
    {
        if (_currentZoomLevel >= farZoomLimit)
            return;
        _currentZoomLevel = Mathf.Min(_currentZoomLevel + delta, farZoomLimit);
        PositionCamera(cam);
    }
}
