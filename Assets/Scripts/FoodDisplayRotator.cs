using UnityEngine;
using UnityEngine.EventSystems;

public class FoodDisplayRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float autoRotationSpeed = 45f;
    [SerializeField] private float interactionSpinDegrees = 360f;
    [SerializeField] private float interactionSpinSpeed = 300f;
    [SerializeField] private float swipeTriggerThreshold = 28f;

    private bool interactionEnabled = true;
    private float queuedSpinDegrees;

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
        bool isUserInteracting = IsUserInteracting();

        if (TryGetSwipeDirection(out float swipeDirection))
        {
            QueueInteractionSpin(swipeDirection);
        }

        if (Mathf.Abs(queuedSpinDegrees) > 0.01f)
        {
            float step = interactionSpinSpeed * Time.deltaTime;
            float rotationStep = Mathf.Min(Mathf.Abs(queuedSpinDegrees), step) * Mathf.Sign(queuedSpinDegrees);
            transform.Rotate(rotationAxis, rotationStep, Space.World);
            queuedSpinDegrees -= rotationStep;
        }
        else if (!isUserInteracting)
        {
            transform.Rotate(rotationAxis, autoRotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void QueueInteractionSpin(float swipeDirection)
    {
        if (Mathf.Abs(swipeDirection) <= 0f)
            return;

        queuedSpinDegrees = -Mathf.Sign(swipeDirection) * interactionSpinDegrees;
    }

    private bool TryGetSwipeDirection(out float swipeDirection)
    {
        swipeDirection = 0f;
        
        if (!interactionEnabled)
            return false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && !IsTouchOverUI(touch.fingerId))
            {
                swipeDirection = touch.deltaPosition.x;
                return Mathf.Abs(swipeDirection) >= swipeTriggerThreshold;
            }

            return false;
        }

        if (Input.GetMouseButton(0) && !IsPointerOverUI())
        {
            swipeDirection = Input.GetAxis("Mouse X") * 20f;
            return Mathf.Abs(swipeDirection) >= swipeTriggerThreshold;
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

    private bool IsUserInteracting()
    {
        if (!interactionEnabled)
            return false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!IsTouchOverUI(touch.fingerId))
                return true;
        }

        return Input.GetMouseButton(0) && !IsPointerOverUI();
    }
}
