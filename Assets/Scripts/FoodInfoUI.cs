using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button viewDetailsButton;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text foodNameText;
    [SerializeField] private TMP_Text foodDescriptionText;
    [SerializeField] private Button closeButton;

    private FoodItem currentFoodItem;

    private void Awake()
    {
        if (viewDetailsButton != null)
            viewDetailsButton.onClick.AddListener(ShowInfoPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(HideInfoPanel);
    }

    private void Start()
    {
        SetDetailsButtonVisible(false);
        HideInfoPanel();
    }

    public void SetFood(FoodItem item)
    {
        currentFoodItem = item;

        if (item == null)
        {
            foodNameText.text = "";
            foodDescriptionText.text = "";
            SetDetailsButtonVisible(false);
            HideInfoPanel();
            return;
        }

        foodNameText.text = item.itemName;
        foodDescriptionText.text = item.description;
        SetDetailsButtonVisible(true);
    }

    public void ShowInfoPanel()
    {
        if (currentFoodItem == null || infoPanel == null)
            return;

        infoPanel.SetActive(true);
    }

    public void HideInfoPanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void SetDetailsButtonVisible(bool isVisible)
    {
        if (viewDetailsButton != null)
            viewDetailsButton.gameObject.SetActive(isVisible);
    }
}