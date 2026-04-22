using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FoodInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button viewDetailsButton;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text foodNameText;
    [SerializeField] private TMP_Text foodDescriptionText;
    [SerializeField] private TMP_Text foodMacrosText;
    [SerializeField] private Button closeButton;

    private FoodItem currentFoodItem;

    private void Awake()
    {
        AutoAssignReferences();

        if (viewDetailsButton != null)
            viewDetailsButton.onClick.AddListener(ShowInfoPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(HideInfoPanel);
    }

    private void Start()
    {
        SetDetailsButtonVisible(false);
    }

    public void SetFood(FoodItem item)
    {
        currentFoodItem = item;

        if (item == null)
        {
            if (foodNameText != null)
                foodNameText.text = "";

            if (foodDescriptionText != null)
                foodDescriptionText.text = "";

            if (foodMacrosText != null)
                foodMacrosText.text = "";

            SetDetailsButtonVisible(false);
            return;
        }

        if (foodNameText != null)
            foodNameText.text = item.itemName;

        if (foodDescriptionText != null)
            foodDescriptionText.text = item.description;

        if (foodMacrosText != null)
            foodMacrosText.text = item.GetMacrosText();

        if (infoPanel != null)
            infoPanel.SetActive(true);

        SetDetailsButtonVisible(true);
    }

    public void ShowInfoPanel()
    {
        if (currentFoodItem == null)
            return;

        if (infoPanel != null)
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

    private void AutoAssignReferences()
    {
        if (infoPanel == null)
        {
            Transform panelTransform = transform.Cast<Transform>()
                .FirstOrDefault(child => child.name.Contains("Panel"));

            if (panelTransform != null)
                infoPanel = panelTransform.gameObject;
        }

        Button[] buttons = GetComponentsInChildren<Button>(true);
        if (viewDetailsButton == null && buttons.Length > 0)
            viewDetailsButton = buttons[0];

        if (closeButton == null && buttons.Length > 1)
            closeButton = buttons[1];

        TMP_Text[] nonButtonTexts = GetComponentsInChildren<TMP_Text>(true)
            .Where(text => text.GetComponentInParent<Button>() == null)
            .ToArray();

        if (foodNameText == null && nonButtonTexts.Length > 0)
            foodNameText = nonButtonTexts[0];

        if (foodDescriptionText == null && nonButtonTexts.Length > 1)
            foodDescriptionText = nonButtonTexts[1];

        if (foodMacrosText == null && nonButtonTexts.Length > 2)
            foodMacrosText = nonButtonTexts[2];

        if (infoPanel == null)
        {
            Debug.LogWarning($"{nameof(FoodInfoUI)} on {name} could not find an info panel automatically. Assign one in the inspector.");
        }
    }
}
