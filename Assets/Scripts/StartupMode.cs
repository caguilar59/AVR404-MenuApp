using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartupMode : MonoBehaviour
{
    [SerializeField] string nextMode = "Scan";
    private bool hasHandledUnsupportedAR;

    void Start()
    {
        UIController.ShowUI("Startup");
    }

    public void OnStartPressed()
    {
        Debug.Log("Start button pressed");

        UIController.ShowUI("Scan");
        InteractionController.EnableMode(nextMode);
    }

    void Update()
    {
        if (!hasHandledUnsupportedAR && ARSession.state == ARSessionState.Unsupported)
        {
            hasHandledUnsupportedAR = true;
            InteractionController.EnableMode("NonAR");
        }
    }
}
