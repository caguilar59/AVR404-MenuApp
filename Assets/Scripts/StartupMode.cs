using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartupMode : MonoBehaviour
{
    [SerializeField] string nextMode = "Scan";

    public void OnStartPressed()
    {
        Debug.Log("Start button pressed");

        UIController.ShowUI("Scan"); // switch UI
        InteractionController.EnableMode(nextMode); // switch mode
    }

    private void OnEnable()
    {
        UIController.ShowUI("Startup");
    }

    void Update()
    {
        if (ARSession.state == ARSessionState.Unsupported)
        {
            InteractionController.EnableMode("NonAR");
        }
        else if (ARSession.state >= ARSessionState.Ready)
        {
            // You can REMOVE this if you only want button control
            // InteractionController.EnableMode(nextMode);
        }
    }
}