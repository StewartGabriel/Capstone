using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandleRelease : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public bool triggerRelease = false;
    private string objectName;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectName = gameObject.name; // Use the GameObject's name to prefix PlayerPrefs keys
    }

    void Update()
    {
        if (triggerRelease && grabInteractable != null && grabInteractable.isSelected)
        {
            if (grabInteractable.interactorsSelecting.Count > 0 &&
                grabInteractable.interactorsSelecting[0] is XRBaseInteractor interactor)
            {
                interactor.EndManualInteraction();
                Debug.Log(objectName + " released.");

                // Save position and rotation to PlayerPrefs with objectName as prefix
                Vector3 pos = transform.position;
                Vector3 rot = transform.eulerAngles;

                PlayerPrefs.SetFloat(objectName + "_Pos_X", pos.x);
                PlayerPrefs.SetFloat(objectName + "_Pos_Y", pos.y);
                PlayerPrefs.SetFloat(objectName + "_Pos_Z", pos.z);

                PlayerPrefs.SetFloat(objectName + "_Rot_X", rot.x);
                PlayerPrefs.SetFloat(objectName + "_Rot_Y", rot.y);
                PlayerPrefs.SetFloat(objectName + "_Rot_Z", rot.z);

                PlayerPrefs.SetInt(objectName + "_HasTransform", 1);
                PlayerPrefs.Save();
            }

            triggerRelease = false;
        }
    }
}
