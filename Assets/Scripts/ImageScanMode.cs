using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageScanMode : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;
    private bool hasEnteredMainMode;

    private void OnEnable()
    {
        hasEnteredMainMode = false;
        UIController.ShowUI("Scan");
    }

    void Update()
    {
        if (!hasEnteredMainMode && imageManager != null && HasActivelyTrackedImage())
        {
            hasEnteredMainMode = true;
            InteractionController.EnableMode("Main");
        }
    }

    private bool HasActivelyTrackedImage()
    {
        foreach (ARTrackedImage trackedImage in imageManager.trackables)
        {
            if (trackedImage != null && trackedImage.trackingState == TrackingState.Tracking)
                return true;
        }

        return false;
    }
}
