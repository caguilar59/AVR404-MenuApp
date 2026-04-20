using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject placePrefab;

    private ARRaycastManager raycastManager;

    private GameObject placedObject;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void PlaceObject(InputValue value)
    {
        if (raycastManager == null || placePrefab == null)
            return;

        Vector2 touchPosition = value.Get<Vector2>();

        // Raycast from the touch position looking for AR planes
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // If object hasn't been placed yet
            if (placedObject == null)
            {
                placedObject = Instantiate(placePrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                // Move existing object
                placedObject.transform.position = hitPose.position;
                placedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
