using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanMode : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;

    private void OnEnable()
    {
        // Show the Scan UI when this mode starts
        UIController.ShowUI("Scan");
    }
}
