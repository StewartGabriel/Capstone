using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextManager : MonoBehaviour
{
    public GameObject noteTextPrefab;     // Prefab with TextMeshProUGUI + ScoreNoteTextDisplay
    public RectTransform scoreArea;       // UI Panel that scrolls notes

    public void AddNote(string symbol, int midiNote, float timeOffset, bool isLeftHand)
    {
        GameObject noteObj = Instantiate(noteTextPrefab, scoreArea);
        var display = noteObj.GetComponent<ScoreNoteTextDisplay>();
        display.Setup(symbol, midiNote, timeOffset, isLeftHand);
    }
}
