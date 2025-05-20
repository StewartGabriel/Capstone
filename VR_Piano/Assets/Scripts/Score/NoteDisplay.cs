using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteDisplay : MonoBehaviour {
    public TextMeshProUGUI baseText;      // Pitch letter
    public TextMeshProUGUI highlightText; // Note duration symbol

    public void SetNote(string symbolChar, string pitchLetter) {
        baseText.text = pitchLetter;         // Note name (e.g., C, D#)
        highlightText.text = symbolChar;     // Musisync symbol
        highlightText.enabled = false;
    }

    public void Highlight(bool highlight) {
        highlightText.enabled = highlight;
    }
}
