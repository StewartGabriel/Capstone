using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandleRelease : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public bool triggerRelease = false;
    private string objectName;
    private int? midiNoteCaptured = null;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectName = gameObject.name;

        Plugin_Init_No_Listening_Board plugin = GameObject.Find("Plugin").GetComponent<Plugin_Init_No_Listening_Board>();
        if (plugin != null)
        {
            plugin.OnMidiInput += OnMidiKeyPress;
        }
    }

    private void OnDestroy()
    {
        Plugin_Init_No_Listening_Board plugin = GameObject.Find("Plugin").GetComponent<Plugin_Init_No_Listening_Board>();
        if (plugin != null)
        {
            plugin.OnMidiInput -= OnMidiKeyPress;
        }
    }

    private void OnMidiKeyPress(int note, int velocity)
    {
        if (velocity > 0 && grabInteractable != null && grabInteractable.isSelected && midiNoteCaptured == null)
        {
            if (objectName == "Piano Handle 1")
            {
                midiNoteCaptured = note;
                Debug.Log($"Captured leftmost MIDI note: {note}");
            }

            triggerRelease = true;
        }
    }

    void Update()
    {
        if (triggerRelease && grabInteractable != null && grabInteractable.isSelected)
        {
            if (grabInteractable.interactorsSelecting.Count > 0 &&
                grabInteractable.interactorsSelecting[0] is XRBaseInteractor interactor)
            {
                interactor.EndManualInteraction();
                Debug.Log(objectName + " released due to MIDI input.");

                Vector3 pos = transform.position;
                Vector3 rot = transform.eulerAngles;

                PlayerPrefs.SetFloat(objectName + "_Pos_X", pos.x);
                PlayerPrefs.SetFloat(objectName + "_Pos_Y", pos.y);
                PlayerPrefs.SetFloat(objectName + "_Pos_Z", pos.z);

                PlayerPrefs.SetFloat(objectName + "_Rot_X", rot.x);
                PlayerPrefs.SetFloat(objectName + "_Rot_Y", rot.y);
                PlayerPrefs.SetFloat(objectName + "_Rot_Z", rot.z);

                PlayerPrefs.SetInt(objectName + "_HasTransform", 1);

                if (objectName == "Piano Handle 1" && midiNoteCaptured.HasValue)
                {
                    PlayerPrefs.SetInt("PianoHandle1_LeftmostNote", midiNoteCaptured.Value);
                    Debug.Log($"Saved MIDI address for Piano Handle 1: {midiNoteCaptured.Value}");
                }

                if (objectName == "Piano Handle 2" && midiNoteCaptured.HasValue)
                {
                    PlayerPrefs.SetInt("PianoHandle2_RightmostNote", midiNoteCaptured.Value);
                    Debug.Log($"Saved MIDI address for Piano Handle 2: {midiNoteCaptured.Value}");
                }

                PlayerPrefs.Save();
            }

            triggerRelease = false;
            midiNoteCaptured = null;
        }
    }
}
