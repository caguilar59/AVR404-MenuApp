using UnityEngine;
using UnityEngine.UI;

public class ImageButtons : MonoBehaviour
{
    [SerializeField] GameObject imageButtonPrefab;
    [SerializeField] ImagesData imagesData;
    [SerializeField] AddPictureMode addPicture;

    void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (ImageInfo image in imagesData.images)
        {
            GameObject obj = Instantiate(imageButtonPrefab, transform);

            RawImage rawImage = obj.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = image.texture;
            }

            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                ImageInfo selectedImage = image;
                button.onClick.AddListener(() => OnClick(selectedImage));
            }
        }
    }

    void OnClick(ImageInfo image)
    {
        addPicture.imageInfo=image;
        InteractionController.EnableMode("AddPicture");
    }
}