using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Handle2Release : MonoBehaviour
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
                Debug.Log("Handle2 released.");

                Vector3 pos = transform.position;
                Vector3 rot = transform.eulerAngles;

                PlayerPrefs.SetFloat("Handle2_Pos_X", pos.x);
                PlayerPrefs.SetFloat("Handle2_Pos_Y", pos.y);
                PlayerPrefs.SetFloat("Handle2_Pos_Z", pos.z);

                PlayerPrefs.SetFloat("Handle2_Rot_X", rot.x);
                PlayerPrefs.SetFloat("Handle2_Rot_Y", rot.y);
                PlayerPrefs.SetFloat("Handle2_Rot_Z", rot.z);

                PlayerPrefs.SetInt("Handle2_HasTransform", 1);
                PlayerPrefs.Save();
            }

            triggerRelease = false;
        }
    }
}
