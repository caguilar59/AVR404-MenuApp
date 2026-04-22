using UnityEngine;

public class FoodDisplayRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 45f;

    public void Configure(Vector3 axis, float speed)
    {
        rotationAxis = axis.sqrMagnitude > 0f ? axis.normalized : Vector3.up;
        rotationSpeed = speed;
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
