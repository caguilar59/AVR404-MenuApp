using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramedPhoto : MonoBehaviour
{
    [SerializeField] private Transform scalerObject;
    [SerializeField] private GameObject imageObject;

    [Header("Highlight")]
    [SerializeField] private GameObject highlightObject;

    [Header("Collision")]
    [SerializeField] private Collider boundingCollider;

    private ImageInfo imageInfo;
    private bool isEditing;
    private int layer;

    private MovePicture movePicture;
    private ResizePicture resizePicture;

    void Awake()
    {
        movePicture = GetComponent<MovePicture>();
        resizePicture = GetComponent<ResizePicture>();

        if (movePicture != null)
            movePicture.enabled = false;

        if (resizePicture != null)
            resizePicture.enabled = false;

        layer = LayerMask.NameToLayer("PlacedObjects");
        Highlight(false);
    }

    public void SetImage(ImageInfo image)
    {
        imageInfo = image;

        Renderer renderer = imageObject.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetTexture("_MainTex", imageInfo.texture);
    }

    public void Highlight(bool show)
    {
        if (highlightObject != null)
            highlightObject.SetActive(show);
    }

    public void BeingEdited(bool editing)
    {
        Highlight(editing);

        if (movePicture != null)
            movePicture.enabled = editing;

        if (resizePicture != null)
            resizePicture.enabled = editing;

        isEditing = editing;
    }

    void OnTriggerStay(Collider other)
    {
        const float spacing = 0.1f;

        if (!isEditing || boundingCollider == null)
            return;

        if (other.gameObject.layer != layer)
            return;

        Bounds bounds = boundingCollider.bounds;

        if (other.bounds.Intersects(bounds))
        {
            Vector3 centerDistance = bounds.center - other.bounds.center;
            Vector3 distOnPlane = Vector3.ProjectOnPlane(centerDistance, transform.forward);

            if (distOnPlane.sqrMagnitude > 0.0001f)
            {
                Vector3 direction = distOnPlane.normalized;
                float distanceToMoveThisFrame = bounds.size.x * spacing;
                transform.Translate(direction * distanceToMoveThisFrame, Space.World);
            }
        }
    }
}