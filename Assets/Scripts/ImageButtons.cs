using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageButtons : MonoBehaviour
{
    [SerializeField] private SelectImageMode selectImage;

    void OnClick(ImageInfo image)
    {
        selectImage.ImageSelected(image);
    }
}