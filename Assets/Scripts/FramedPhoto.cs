using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramedPhoto : MonoBehaviour
{
    [SerializeField] Transform scalerObject;
    [SerializeField] GameObject imageObject;

    ImageInfo imageInfo;

    public void SetImage(ImageInfo image)
    {
        imageInfo = image;
Renderer renderer = imageObject.GetComponent<Renderer>();
 Material material = renderer.material; 
 material.SetTexture("_MainTex", imageInfo.texture);

    }
}