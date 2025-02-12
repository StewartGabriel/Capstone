using UnityEngine;

public class CenterCanvas : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float distanceFromCamera = 1f; // Distance in front of the camera
    public float verticalOffset = -0.2f; // Offset to position the canvas slightly lower

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Find the main camera if not assigned
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Position the canvas in front of the camera and slightly lower
            Vector3 forwardPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
            Vector3 offset = mainCamera.transform.up * verticalOffset; // Offset downward
            transform.position = forwardPosition + offset;

            // Match the camera's rotation
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
