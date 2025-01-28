using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class PianoAnchoringManager : MonoBehaviour
{
    private ARAnchorManager anchorManager;
    private ARAnchor currentAnchor;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isMoveAllowed = false; // Controls when the player can move the object

    void Start()
    {
        anchorManager = FindObjectOfType<ARAnchorManager>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (anchorManager == null)
        {
            Debug.LogError("ARAnchorManager not found in the scene! Disabling script.");
            enabled = false;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false; // Disable grabbing initially
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    // Called when the button is pressed
    public void AllowMoveOnce()
    {
        if (!isMoveAllowed) 
        {
            isMoveAllowed = true;
            grabInteractable.enabled = true; // Enable grabbing
            Debug.Log("Player can move the keyboard.");
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isMoveAllowed)
        {
            PlaceAnchor();
            grabInteractable.enabled = false; // Disable grabbing after first move
            isMoveAllowed = false;
            Debug.Log("Keyboard placed and locked.");
        }
    }

    private void PlaceAnchor()
    {
        if (currentAnchor == null)
        {
            currentAnchor = GetComponent<ARAnchor>();
            if (currentAnchor == null)
            {
                currentAnchor = gameObject.AddComponent<ARAnchor>();
                Debug.Log("New ARAnchor component added.");
            }
            else
            {
                Debug.Log("ARAnchor already exists.");
            }
        }
        else
        {
            Debug.Log("Anchor already placed.");
        }
    }

    public void RemoveAnchor()
    {
        if (currentAnchor != null)
        {
            Destroy(currentAnchor);
            currentAnchor = null;
            Debug.Log("Anchor removed.");
        }
    }
}
