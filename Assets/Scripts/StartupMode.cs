using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartupMode : MonoBehaviour
{
    [SerializeField] string nextMode = "Scan";
    [SerializeField] string startupPanelName = "Startup";
    [SerializeField] string nextPanelName = "Scan";
    [SerializeField] float autoAdvanceDelay = 1.5f;

    private bool hasHandledUnsupportedAR;
    private Coroutine autoAdvanceRoutine;

    private void OnEnable()
    {
        UIController.ShowUI(startupPanelName);

        if (autoAdvanceRoutine != null)
            StopCoroutine(autoAdvanceRoutine);

        autoAdvanceRoutine = StartCoroutine(AutoAdvanceToNextMode());
    }

    private void OnDisable()
    {
        if (autoAdvanceRoutine != null)
        {
            StopCoroutine(autoAdvanceRoutine);
            autoAdvanceRoutine = null;
        }
    }

    public void OnStartPressed()
    {
        AdvanceToNextMode();
    }

    private IEnumerator AutoAdvanceToNextMode()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        AdvanceToNextMode();
    }

    private void AdvanceToNextMode()
    {
        if (!isActiveAndEnabled)
            return;

        UIController.ShowUI(nextPanelName);
        InteractionController.EnableMode(nextMode);
        autoAdvanceRoutine = null;
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
