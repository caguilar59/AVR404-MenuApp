using UnityEngine;

[System.Serializable]
public class FoodItem
{
    public string markerName;
    public string itemName;

    [TextArea(3, 6)]
    public string description;

    public GameObject foodPrefab;
}