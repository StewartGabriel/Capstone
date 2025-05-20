using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNoteDisplay : MonoBehaviour
{
    public SpriteRenderer noteRenderer;
    public Sprite[] noteSprites; // 0: quarter, 1: half, 2: whole, etc.

    public float staffSpacing = 0.1f;
    public float horizontalScale = 1.0f;

    public void Setup(int midiNote, float duration, float timeOffset, bool isLeftHand)
    {
        transform.localPosition = GetNotePosition(midiNote, timeOffset, isLeftHand);
        noteRenderer.sprite = GetSpriteForDuration(duration);
    }

    private Vector3 GetNotePosition(int midiNote, float timeOffset, bool isLeftHand)
    {
        float y = NoteToStaffY(midiNote);
        float x = timeOffset * horizontalScale;
        float z = isLeftHand ? -0.1f : 0.1f; // Separate layers for left/right hand
        return new Vector3(x, y, z);
    }

    private float NoteToStaffY(int midiNote)
    {
        return (midiNote - 60) * staffSpacing; // Middle C is 60
    }

    private Sprite GetSpriteForDuration(float duration)
    {
        if (duration < 0.3f) return noteSprites[0]; // 16th
        if (duration < 0.6f) return noteSprites[1]; // 8th
        if (duration < 1.2f) return noteSprites[2]; // quarter
        return noteSprites[3]; // half or longer
    }
}
