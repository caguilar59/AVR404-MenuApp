using UnityEngine;
using UnityEngine.EventSystems;

public class FoodDisplayRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float autoRotationSpeed = 45f;
    [SerializeField] private float swipeRotationMultiplier = 0.2f;

    private bool interactionEnabled = true;

    public void Configure(Vector3 axis, float speed)
    {
        rotationAxis = axis.sqrMagnitude > 0f ? axis.normalized : Vector3.up;
        autoRotationSpeed = speed;
    }

    public void SetInteractionEnabled(bool isEnabled)
    {
        interactionEnabled = isEnabled;
    }

    private void Update()
    {
        if (TryGetSwipeDelta(out float swipeDelta))
        {
            transform.Rotate(rotationAxis, -swipeDelta * swipeRotationMultiplier, Space.World);
            return;
        }

        transform.Rotate(rotationAxis, autoRotationSpeed * Time.deltaTime, Space.Self);
    }

    private bool TryGetSwipeDelta(out float swipeDelta)
    {
        swipeDelta = 0f;

        if (!interactionEnabled)
            return false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && !IsTouchOverUI(touch.fingerId))
            {
                swipeDelta = touch.deltaPosition.x;
                return Mathf.Abs(swipeDelta) > 0f;
            }

            return false;
        }

        if (Input.GetMouseButton(0) && !IsPointerOverUI())
        {
            swipeDelta = Input.GetAxis("Mouse X") * 20f;
            return Mathf.Abs(swipeDelta) > 0f;
        }

        return false;
    }

    private static bool IsTouchOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    private static bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
