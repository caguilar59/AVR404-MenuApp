using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    GameObject placePrefab;

    ARRaycastManager raycastManager;

    GameObject placedObject;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void PlaceObject(InputValue value)
    {
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