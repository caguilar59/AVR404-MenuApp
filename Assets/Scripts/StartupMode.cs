using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartupMode : MonoBehaviour
{

    [SerializeField] string nextMode = "Scan";

    private void OnEnable()
    {
        UIController.ShowUI("Startup");
    }



    // Update is called once per frame
    void Update()
    {
        if (ARSession.state == ARSessionState.Unsupported)
        {
            InteractionController.EnableMode("NonAR");
        }
        else if (ARSession.state >= ARSessionState.Ready)
        {
            InteractionController.EnableMode(nextMode);
        }
    }
}