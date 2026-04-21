using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMode : MonoBehaviour
{
    [SerializeField] private EditPictureMode editMode;
    [SerializeField] private SelectImageMode selectImage;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        UIController.ShowUI("Main");
    }

    public void OnSelectObject(InputValue value)
    {
        Vector2 touchPosition = value.Get<Vector2>();
        FindObjectToEdit(touchPosition);
    }

    void FindObjectToEdit(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            FramedPhoto picture = hit.collider.GetComponentInParent<FramedPhoto>();

            if (picture != null)
            {
                editMode.currentPicture = picture;
                InteractionController.EnableMode("EditPicture");
            }
        }
    }

    public void SelectImageToAdd()
    {
        selectImage.isReplacing = false;
        InteractionController.EnableMode("SelectImage");
    }
}