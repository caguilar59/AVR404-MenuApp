using UnityEngine;

public class TitleMode : MonoBehaviour
{
    [SerializeField] private string nextMode = "Startup";
    [SerializeField] private string titlePanelName = "Title";
    [SerializeField] private string nextPanelName = "Startup";

    private void OnEnable()
    {
        UIController.ShowUI(titlePanelName);
    }

    public void OnStartPressed()
    {
        UIController.ShowUI(nextPanelName);
        InteractionController.EnableMode(nextMode);
    }
}
