using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AddPictureMode : MonoBehaviour
{
    public ImageInfo imageInfo;

    [SerializeField] private ARRaycastManager raycaster;
    [SerializeField] private GameObject placedPrefab;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;

    private void OnEnable()
    {
        UIController.ShowUI("AddPicture");
        spawnedObject = null;
    }

    public void OnPlaceObject(InputValue value)
    {
        Vector2 touchPosition = value.Get<Vector2>();
        PlaceObject(touchPosition);
    }

    void PlaceObject(Vector2 touchPosition)
    {
        if (raycaster.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Face outward from surface
            Vector3 normal = -hitPose.up;
            Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(placedPrefab, hitPose.position, rotation);
                spawnedObject.transform.SetParent(transform.parent);

                FramedPhoto photo = spawnedObject.GetComponent<FramedPhoto>();
                if (photo != null)
                {
                    photo.SetImage(imageInfo);
                }
            }
            else
            {
                spawnedObject.transform.SetPositionAndRotation(hitPose.position, rotation);
            }
        }
    }
}