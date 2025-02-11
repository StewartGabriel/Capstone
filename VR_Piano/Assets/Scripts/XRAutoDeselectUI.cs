using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class XRAutoDeselectUI : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor leftHandRayInteractor;  // Assign your left hand interactor
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightHandRayInteractor; // Assign your right hand interactor

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null && !IsXRPointerOverUI())
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private bool IsXRPointerOverUI()
    {
        return IsPointerOverUI(leftHandRayInteractor) || IsPointerOverUI(rightHandRayInteractor);
    }

    private bool IsPointerOverUI(UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor interactor)
    {
        if (interactor != null && interactor.TryGetCurrentUIRaycastResult(out RaycastResult result))
        {
            return result.gameObject != null;
        }
        return false;
    }
}
