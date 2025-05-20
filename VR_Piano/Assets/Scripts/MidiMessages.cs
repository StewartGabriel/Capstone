using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System.Threading;
using System.Threading.Tasks;

public class MidiMessages : MonoBehaviour
{

    public ScoreTextManager scoreTextManager;
    private float cumulativeTime = 0f;
    private Dictionary<int, float> noteOnTimestamps = new Dictionary<int, float>();

    [SerializeField] private TextAsset[] songFiles; // TextAsset songs array (extracted midi data)

    public TalkingBoard toNoteCallback; // create reference to NoteCallback
    public ListeningBoard playerBoard;
    void Start()
    {
        toNoteCallback = playerBoard.talkingboard;
        if (toNoteCallback == null)
        {
            Debug.LogError("NoteCallback not referenced correctly");
        }
    }

    public async void PlaySong(int songIndex, bool left_enabled, bool right_enabled, float tempo_multiplier)
    {
        if (songIndex >= 1 && songIndex * 2 <= songFiles.Length)
        {
            TextAsset leftPart = songFiles[(songIndex * 2) - 2]; // Even index
            TextAsset rightPart = songFiles[(songIndex * 2) - 1]; // Odd index

            List<Task> playTasks = new List<Task>();

            if (left_enabled && leftPart != null)
            {
                playTasks.Add(ExtractMidiData(leftPart, true, tempo_multiplier));
            }

            if (right_enabled && rightPart != null)
            {
                playTasks.Add(ExtractMidiData(rightPart, false, tempo_multiplier));
            }

            if (playTasks.Count > 0)
            {
                Debug.Log($"Playing song {songIndex} with tempo x{tempo_multiplier}. Left: {left_enabled}, Right: {right_enabled}");
                await Task.WhenAll(playTasks);
            }
            else
            {
                Debug.LogError("No valid hand parts enabled or files missing.");
            }
        }
        else
        {
            Debug.LogError("Song index out of range: " + songIndex);
        }
    }

    public async Task ExtractMidiData(TextAsset midiMessages, bool hand, float tempo_multiplier)
    {
        // Reads all the lines of the file
        string[] lines = midiMessages.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        int i = 0;
        foreach (string line in lines)
        {
            i++;
            string[] index = line.Split(' ');

            if (index.Length >= 4)
            {
                string onOff = index[0];
                int note = int.Parse(index[1]);
                int velocity = int.Parse(index[2]);
                int timeDelay = int.Parse(index[3]);

                float adjustedDelay = timeDelay / tempo_multiplier;
                await Task.Delay((int)adjustedDelay);
                cumulativeTime += adjustedDelay / 1000f;

                if (onOff == "on")
                {
                    toNoteCallback.InterpretMidi(note, velocity, hand);
                    noteOnTimestamps[note] = cumulativeTime; // Track note on time
                }
                else
                {
                    toNoteCallback.InterpretMidi(note, 0, hand);
                    float start = noteOnTimestamps.ContainsKey(note) ? noteOnTimestamps[note] : cumulativeTime;
                    float duration = cumulativeTime - start;

                    string symbol = GetSymbolForDuration(duration, isRest: false);
                    scoreTextManager.AddNote(symbol, note, start, hand);
                    noteOnTimestamps.Remove(note);
                }
            }
        }
    }

    private string GetSymbolForDuration(float duration, bool isRest = false)
    {
        if (isRest)
        {
            if (duration < 0.25f) return "\u0053"; // 16th note rest
            if (duration < 0.5f)  return "\u0045"; // Eighth note rest
            if (duration < 1.0f)  return "\u0051"; // Quarter note rest
            if (duration < 2.0f)  return "\u0057"; // Half note rest
            return "\u0048";                         // Whole rest
        }
        else
        {
            if (duration < 0.25f) return "\u0073"; // 16th note
            if (duration < 0.5f)  return "\u0065"; // Eighth note
            if (duration < 1.0f)  return "\u0071"; // Quarter note
            if (duration < 2.0f)  return "\u0068"; // Half note
            return "\u0077";                       // Whole note
        }
    }

    
}
