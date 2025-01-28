using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PianoAnchoringManager : MonoBehaviour
{
    private ARAnchorManager anchorManager;
    private ARAnchor currentAnchor;

    void Start()
    {
        anchorManager = FindObjectOfType<ARAnchorManager>();

        if (anchorManager == null)
        {
            Debug.LogError("ARAnchorManager not found in the scene!");
        }
    }

    public void PlaceAnchor()
    {
        if (currentAnchor == null)
        {
            Pose objectPose = new Pose(transform.position, transform.rotation);

            // Add a new anchor at the current object's position
            currentAnchor = anchorManager.AddAnchor(objectPose);

            if (currentAnchor != null)
            {
                Debug.Log("Anchor placed successfully.");
            }
            else
            {
                Debug.LogError("Failed to place anchor.");
            }
        }
        else
        {
            Debug.Log("Anchor already exists.");
        }
    }

    public void RemoveAnchor()
    {
        if (currentAnchor != null)
        {
            Destroy(currentAnchor.gameObject);
            currentAnchor = null;
            Debug.Log("Anchor removed.");
        }
    }
}
