using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageScanMode : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;

    private void OnEnable()
    {
        // Show the Scan UI when this mode starts
        UIController.ShowUI("Scan");
    }

    private void Update()
    {
        if (imageManager == null) return;

        foreach (var trackedImage in imageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                InteractionController.EnableMode("Main");
                return;
            }
        }
    }
}