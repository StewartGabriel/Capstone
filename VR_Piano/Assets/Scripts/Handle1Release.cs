using UnityEngine;


public class Handle1Release : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public bool triggerRelease = false;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Update()
    {
        if (triggerRelease && grabInteractable != null && grabInteractable.isSelected)
        {
            if (grabInteractable.interactorsSelecting.Count > 0)
            {
                // Cast to XRBaseInteractor
                if (grabInteractable.interactorsSelecting[0] is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
                {
                    interactor.EndManualInteraction();
                    Debug.Log("Object released by condition.");
                }
            }

            Handle1PositionStorage.savedPosition = transform.position;
            Handle1PositionStorage.hasSavedPosition = true;

            triggerRelease = false;
        }
    }
}
