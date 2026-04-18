using System.Collections.Generic;
using UnityEngine;

public class FoodDatabase : MonoBehaviour
{
    [SerializeField] private List<FoodItem> foodItems = new List<FoodItem>();

    public FoodItem GetFoodByMarkerName(string markerName)
    {
        foreach (FoodItem item in foodItems)
        {
            if (item.markerName.Trim().ToLower() == markerName.Trim().ToLower())
                return item;
        }

        return null;
    }
}