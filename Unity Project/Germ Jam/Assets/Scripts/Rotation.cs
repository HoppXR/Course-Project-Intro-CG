using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    [SerializeField] private float zRotation;

    private void Update()
    {
        transform.Rotate(xRotation, yRotation, zRotation);
    }
}
