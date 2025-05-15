using System.Collections.Generic;
using UnityEngine;

public class MidiScoreGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset midiData;

    public class MidiNote
    {
        public int noteNumber;
        public float startTime;
        public float duration;
        public int velocity;
    }

    public List<MidiNote> GenerateScore()
    {
        return GenerateScoreFromFile(midiData);
    }

    public List<MidiNote> GenerateScoreFromFile(TextAsset midiData)
    {
        List<MidiNote> notes = new List<MidiNote>();
        string[] lines = midiData.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        Dictionary<int, float> activeNotes = new();
        float currentTime = 0f;

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            if (parts.Length < 4) continue;

            string onOff = parts[0];
            if (!int.TryParse(parts[1], out int note) ||
                !int.TryParse(parts[2], out int velocity) ||
                !int.TryParse(parts[3], out int delay))
                continue;

            currentTime += delay;

            if (onOff == "on")
            {
                if (!activeNotes.ContainsKey(note))
                    activeNotes[note] = currentTime;
            }
            else if (onOff == "off" && activeNotes.ContainsKey(note))
            {
                float start = activeNotes[note];
                float duration = currentTime - start;

                notes.Add(new MidiNote
                {
                    noteNumber = note,
                    startTime = start,
                    duration = duration,
                    velocity = velocity
                });

                activeNotes.Remove(note);
            }
        }

        return notes;
    }

    public static string NoteNumberToName(int midiNote)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        int octave = (midiNote / 12) - 1;
        string note = noteNames[midiNote % 12];
        return note + octave;
    }
}
