using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageFoodManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private FoodDatabase foodDatabase;
    [SerializeField] private FoodInfoUI foodInfoUI;

    [Header("UI")]
    [SerializeField] private GameObject scanUI;
    [SerializeField] private GameObject mainUI;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.1f, 0f);

    private Dictionary<string, GameObject> spawnedFoods = new Dictionary<string, GameObject>();
    private string currentMarkerName = "";

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string markerName = trackedImage.referenceImage.name;

            if (spawnedFoods.ContainsKey(markerName))
            {
                Destroy(spawnedFoods[markerName]);
                spawnedFoods.Remove(markerName);
            }
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        string markerName = trackedImage.referenceImage.name;

        if (trackedImage.trackingState != TrackingState.Tracking)
            return;

        FoodItem item = foodDatabase.GetFoodByMarkerName(markerName);

        if (item == null)
        {
            Debug.LogWarning("No food item found for marker: " + markerName);
            return;
        }

        currentMarkerName = markerName;

        if (!spawnedFoods.ContainsKey(markerName))
        {
            Vector3 spawnPosition = trackedImage.transform.position + spawnOffset;
            Quaternion spawnRotation = trackedImage.transform.rotation;

            GameObject spawnedFood = Instantiate(item.foodPrefab, spawnPosition, spawnRotation);
            spawnedFood.transform.SetParent(trackedImage.transform);

            spawnedFoods.Add(markerName, spawnedFood);
        }

        if (foodInfoUI != null)
            foodInfoUI.SetFood(item);

        if (scanUI != null)
            scanUI.SetActive(false);

        if (mainUI != null)
            mainUI.SetActive(true);
    }

    public void ResetTrackingUI()
    {
        currentMarkerName = "";

        if (scanUI != null)
            scanUI.SetActive(true);

        if (mainUI != null)
            mainUI.SetActive(false);

        if (foodInfoUI != null)
            foodInfoUI.SetFood(null);
    }
}