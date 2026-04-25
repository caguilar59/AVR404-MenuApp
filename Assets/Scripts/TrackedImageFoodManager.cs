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
    [SerializeField] private string scanPanelName = "Scan";
    [SerializeField] private string trackedFoodPanelName = "FoodInfo";

    [Header("Modes")]
    [SerializeField] private string scanModeName = "Scan";
    [SerializeField] private string trackedFoodModeName = "FoodInfo";

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.1f, 0f);

    [Header("Display Animation")]
    [SerializeField] private bool rotateSpawnedFood = true;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

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

            if (currentMarkerName == markerName)
            {
                ResetTrackingUIIfNothingTracked();
            }
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        string markerName = trackedImage.referenceImage.name;

        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            GameObject spawnedFood;
            if (spawnedFoods.TryGetValue(markerName, out spawnedFood) && spawnedFood != null)
                spawnedFood.SetActive(false);

            if (currentMarkerName == markerName)
                ResetTrackingUIIfNothingTracked();

            return;
        }

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
            ConfigureSpawnedFood(spawnedFood);

            spawnedFoods.Add(markerName, spawnedFood);
        }
        else if (spawnedFoods[markerName] != null)
        {
            spawnedFoods[markerName].SetActive(true);
        }

        if (foodInfoUI != null)
            foodInfoUI.SetFood(item);

        UpdateInteractiveFood();

        if (InteractionController.IsInitialized)
            InteractionController.EnableMode(trackedFoodModeName);

        if (UIController.IsInitialized)
        {
            UIController.ShowUI(trackedFoodPanelName);
        }
        else
        {
            if (scanUI != null)
                scanUI.SetActive(false);

            if (mainUI != null)
                mainUI.SetActive(true);
        }
    }

    public void ResetTrackingUI()
    {
        currentMarkerName = "";

        if (InteractionController.IsInitialized)
            InteractionController.EnableMode(scanModeName);

        if (UIController.IsInitialized)
        {
            UIController.ShowUI(scanPanelName);
        }
        else
        {
            if (scanUI != null)
                scanUI.SetActive(true);

            if (mainUI != null)
                mainUI.SetActive(false);
        }

        if (foodInfoUI != null)
            foodInfoUI.SetFood(null);

        UpdateInteractiveFood();
    }

    private void ConfigureSpawnedFood(GameObject spawnedFood)
    {
        if (!rotateSpawnedFood || spawnedFood == null)
            return;

        FoodDisplayRotator rotator = spawnedFood.GetComponent<FoodDisplayRotator>();
        if (rotator == null)
            rotator = spawnedFood.AddComponent<FoodDisplayRotator>();

        rotator.Configure(rotationAxis, rotationSpeed);
    }

    private void UpdateInteractiveFood()
    {
        foreach (KeyValuePair<string, GameObject> entry in spawnedFoods)
        {
            if (entry.Value == null)
                continue;

            FoodDisplayRotator rotator = entry.Value.GetComponent<FoodDisplayRotator>();
            if (rotator == null)
                continue;

            bool isCurrentFood = !string.IsNullOrEmpty(currentMarkerName) &&
                entry.Key == currentMarkerName &&
                entry.Value.activeInHierarchy;

            rotator.SetInteractionEnabled(isCurrentFood);
        }
    }

    private void ResetTrackingUIIfNothingTracked()
    {
        if (HasAnyActiveTrackedImage())
            return;

        ResetTrackingUI();
    }

    private bool HasAnyActiveTrackedImage()
    {
        if (trackedImageManager == null)
            return false;

        foreach (ARTrackedImage trackedImage in trackedImageManager.trackables)
        {
            if (trackedImage != null && trackedImage.trackingState == TrackingState.Tracking)
                return true;
        }

        return false;
    }
}
