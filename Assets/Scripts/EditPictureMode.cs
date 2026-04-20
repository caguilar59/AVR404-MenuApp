using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditPictureMode : MonoBehaviour
{
    public FramedPhoto currentPicture;

    [SerializeField] private SelectImageMode selectImage;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        UIController.ShowUI("EditPicture");

        if (currentPicture)
            currentPicture.BeingEdited(true);
    }

    void OnDisable()
    {
        if (currentPicture)
            currentPicture.BeingEdited(false);
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

            if (picture != null && picture != currentPicture)
            {
                if (currentPicture != null)
                    currentPicture.BeingEdited(false);

                currentPicture = picture;
                picture.BeingEdited(true);
            }
        }
    }

    public void SelectImageToReplace()
    {
        selectImage.isReplacing = true;
        InteractionController.EnableMode("SelectImage");
    }
}
