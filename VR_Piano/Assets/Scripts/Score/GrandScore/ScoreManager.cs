using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform scoreRoot; // Container for scrolling
    private float currentScrollOffset = 0f;
    public float scrollSpeed = 1.0f;

    private List<ScoreNoteDisplay> activeNotes = new();

    void Update()
    {
        // Scroll the entire score
        currentScrollOffset += Time.deltaTime * scrollSpeed;
        scoreRoot.localPosition = new Vector3(-currentScrollOffset, 0, 0);
    }

    public void AddNote(int midiNote, float duration, float timeOffset, bool isLeftHand)
    {
        GameObject obj = Instantiate(notePrefab, scoreRoot);
        ScoreNoteDisplay noteDisplay = obj.GetComponent<ScoreNoteDisplay>();
        noteDisplay.Setup(midiNote, duration, timeOffset, isLeftHand);
        activeNotes.Add(noteDisplay);
    }
}
