using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
        if (!hasEnteredMainMode && imageManager != null && imageManager.trackables.count > 0)
        {
            hasEnteredMainMode = true;
            InteractionController.EnableMode("Main");
        }
    }
}
