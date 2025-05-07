using System.Collections;
using UnityEngine;

public class Handle1Release : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    public bool triggerRelease = false;

    void Update()
    {
        if (triggerRelease && grabInteractable.isSelected)
        {
            var interactor = grabInteractable.selectingInteractor;
            if (interactor != null)
            {
                interactor.EndManualInteraction();
                Debug.Log("Object released by condition.");
            }

            // Save the position to static storage
            Handle1PositionStorage.savedPosition = transform.position;
            Handle1PositionStorage.hasSavedPosition = true;

            triggerRelease = false; // Reset
        }
    }
}

