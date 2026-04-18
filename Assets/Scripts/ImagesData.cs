using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ImageInfo
{
    public int width;
    public Texture texture;
    public int height;
}

public class ImagesData : MonoBehaviour
{
    public List<ImageInfo> images = new List<ImageInfo>();
}