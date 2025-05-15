using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MidiMessages : MonoBehaviour
{
    [SerializeField] private TextAsset[] songFiles; // MIDI-like text files
    public TalkingBoard toNoteCallback;
    public ListeningBoard playerBoard;
    public ScoreVisualizer scoreVisualizer;
    public MidiScoreGenerator midiParser;

    void Start()
    {
        toNoteCallback = playerBoard.talkingboard;
        if (toNoteCallback == null)
            Debug.LogError("NoteCallback not referenced correctly");
    }

    public async void PlaySong(int songIndex, bool left_enabled, bool right_enabled, float tempo_multiplier)
    {
        if (songIndex < 1 || songIndex * 2 > songFiles.Length)
        {
            Debug.LogError("Song index out of range: " + songIndex);
            return;
        }

        TextAsset leftPart = songFiles[(songIndex * 2) - 2];
        TextAsset rightPart = songFiles[(songIndex * 2) - 1];

        List<MidiScoreGenerator.MidiNote> combinedNotes = new();

        if (left_enabled && leftPart != null)
            combinedNotes.AddRange(midiParser.GenerateScoreFromFile(leftPart));

        if (right_enabled && rightPart != null)
            combinedNotes.AddRange(midiParser.GenerateScoreFromFile(rightPart));

        // Sort notes by startTime to sync playback
        combinedNotes.Sort((a, b) => a.startTime.CompareTo(b.startTime));

        // Render full score
        scoreVisualizer.RenderScore(combinedNotes);

        // Begin playback per hand
        List<Task> playTasks = new();
        if (left_enabled && leftPart != null)
            playTasks.Add(ExtractMidiData(leftPart, true, tempo_multiplier));
        if (right_enabled && rightPart != null)
            playTasks.Add(ExtractMidiData(rightPart, false, tempo_multiplier));

        if (playTasks.Count > 0)
        {
            Debug.Log($"Playing song {songIndex} | Tempo x{tempo_multiplier} | L: {left_enabled} R: {right_enabled}");
            await Task.WhenAll(playTasks);
        }
        else
        {
            Debug.LogError("No valid hand parts enabled or files missing.");
        }
    }

    public async Task ExtractMidiData(TextAsset midiMessages, bool hand, float tempo_multiplier)
    {
        string[] lines = midiMessages.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
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
            float adjustedDelay = delay / tempo_multiplier;

            await Task.Delay((int)adjustedDelay);

            // Highlight current time
            scoreVisualizer?.UpdatePlaybackHighlight(currentTime);

            if (toNoteCallback != null)
            {
                if (onOff == "on")
                    toNoteCallback.InterpretMidi(note, velocity, hand); // KeyDown
                else
                    toNoteCallback.InterpretMidi(note, 0, hand);        // KeyUp
            }
        }
    }
}