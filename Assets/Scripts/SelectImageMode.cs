using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageMode : MonoBehaviour
{
    [SerializeField] private AddPictureMode addPicture;
    [SerializeField] private EditPictureMode editPicture;

    public bool isReplacing = false;

    private void OnEnable()
    {
        UIController.ShowUI("SelectImage");
    }

    public void ImageSelected(ImageInfo image)
    {
        if (isReplacing)
        {
            if (editPicture != null && editPicture.currentPicture != null)
            {
                editPicture.currentPicture.SetImage(image);
            }

            InteractionController.EnableMode("EditPicture");
        }
        else
        {
            addPicture.imageInfo = image;
            InteractionController.EnableMode("AddPicture");
        }
    }
}