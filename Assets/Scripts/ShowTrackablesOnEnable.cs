using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;

public class ShowTrackablesOnEnable : MonoBehaviour
{
    [SerializeField] private XROrigin sessionOrigin;
    private ARPlaneManager planeManager;
    private ARPointCloudManager cloudManager;
    private bool isStarted;

    private void Awake()
    {
        if (sessionOrigin != null)
        {
            planeManager = sessionOrigin.GetComponent<ARPlaneManager>();
            cloudManager = sessionOrigin.GetComponent<ARPointCloudManager>();
        }

        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();

        if (cloudManager == null)
            cloudManager = FindObjectOfType<ARPointCloudManager>();
    }

    private void Start()
    {
        isStarted = true;
    }

    private void OnEnable()
    {
        ShowTrackables(true);
    }

    private void OnDisable()
    {
        if (isStarted)
        {
            ShowTrackables(false);
        }
    }


    private void ShowTrackables(bool show)
    {
        if (cloudManager)
        {
            cloudManager.SetTrackablesActive(show);
            cloudManager.enabled = show;
        }
        if (planeManager)
        {
            planeManager.SetTrackablesActive(show);
            planeManager.enabled = show;
        }
    }
}
