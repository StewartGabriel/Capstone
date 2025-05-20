using UnityEngine;

public class NoteEvent {
    public int pitch;
    public float startTime;
    public float duration;
    public bool isRest;
    public string symbol;
    public bool isLeftHand; // <-- Add this line

    public NoteEvent(int pitch, float startTime, float duration, bool isRest = false, string symbol = null, bool isLeftHand = false) {
        this.pitch = pitch;
        this.startTime = startTime;
        this.duration = duration;
        this.isRest = isRest;
        this.symbol = symbol;
        this.isLeftHand = isLeftHand;
    }
}
