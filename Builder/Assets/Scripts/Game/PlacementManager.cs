using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _placeableObjectPrefab;

    private GameObject currentPlaceableObject;
    private float mouseWheelRotation;

    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey();

        // Move the object
        if (currentPlaceableObject != null)
        {
            MoveObjectToMouse();
            RotateFromMouseWheel();
            ReleasedIfClicked();
        }
    }

    private void ReleasedIfClicked()
    {
        // Release if Clicked
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
        }
    }

    private void RotateFromMouseWheel()
    {
        // Rotate from mouse wheel
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void MoveObjectToMouse()
    {
        // Ray cast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.point);
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.blue);

            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            // TODO: Check to see if normal vector is different from the UP vector
        }
    }

    private void HandleNewObjectHotkey()
    {
        // Instantiate the object
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPlaceableObject == null)
            {
                currentPlaceableObject = Instantiate(_placeableObjectPrefab);
            }
            else
            {
                Destroy(currentPlaceableObject);
            }
        }
    }
}
