
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float rotationSpeed = 120f;

    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}
