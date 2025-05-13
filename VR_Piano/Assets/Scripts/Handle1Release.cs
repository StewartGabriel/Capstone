using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Handle1Release : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public bool triggerRelease = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (triggerRelease && grabInteractable != null && grabInteractable.isSelected)
        {
            if (grabInteractable.interactorsSelecting.Count > 0 &&
                grabInteractable.interactorsSelecting[0] is XRBaseInteractor interactor)
            {
                interactor.EndManualInteraction();
                Debug.Log("Handle1 released.");

                // Save position and rotation to PlayerPrefs
                Vector3 pos = transform.position;
                Vector3 rot = transform.eulerAngles;

                PlayerPrefs.SetFloat("Handle1_Pos_X", pos.x);
                PlayerPrefs.SetFloat("Handle1_Pos_Y", pos.y);
                PlayerPrefs.SetFloat("Handle1_Pos_Z", pos.z);

                PlayerPrefs.SetFloat("Handle1_Rot_X", rot.x);
                PlayerPrefs.SetFloat("Handle1_Rot_Y", rot.y);
                PlayerPrefs.SetFloat("Handle1_Rot_Z", rot.z);

                PlayerPrefs.SetInt("Handle1_HasTransform", 1);
                PlayerPrefs.Save();
            }

            triggerRelease = false;
        }
    }
}
