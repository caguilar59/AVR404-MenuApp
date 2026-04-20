using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MovePicture : MonoBehaviour
{
    private ARRaycastManager raycaster;
    private Camera mainCamera;
    private int layerMask;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycaster = FindObjectOfType<ARRaycastManager>();
        mainCamera = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");
    }

    public void OnMoveObject(InputValue value)
    {
        if (!enabled) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 touchPosition = value.Get<Vector2>();
        MoveObject(touchPosition);
    }

    void MoveObject(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            if (raycaster.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                ARRaycastHit hit = hits[0];
                Vector3 position = hit.pose.position;
                Vector3 normal = -hit.pose.up;
                Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);

                transform.position = position;
                transform.rotation = rotation;
            }
        }
    }
}