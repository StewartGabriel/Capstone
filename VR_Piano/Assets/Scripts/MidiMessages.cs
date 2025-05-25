using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System.Threading;
using System.Threading.Tasks;

public class MidiMessages : MonoBehaviour
{
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

                // Scale delay by tempo multiplier
                float adjustedDelay = timeDelay / tempo_multiplier;
                await Task.Delay((int)adjustedDelay); // This might be the cause of our sound delay issues?

                // Sends the extracted data to NoteCallback component
                if (toNoteCallback != null)
                {
                    if (onOff == "on")
                    {
                        //await Task.Delay(adjustedDelay);
                        toNoteCallback.InterpretMidi(note, velocity, hand); // KeyDown
                    }

                    else
                    {
                        //await Task.Delay(adjustedDelay);
                        toNoteCallback.InterpretMidi(note, 0, hand); // KeyUp
                    }
                }

                //// Debug Log string format
                //string debugData = $"Note on/off: {onOff}, Midi Number: {note}, Velocity: {velocity}, Time Delay: {adjustedDelay}, Line: {i}";

                //Debug.Log(debugData);
            }
        }

        ListeningBoard noteCallback = GameObject.Find("Board Listening").GetComponent<ListeningBoard>();
       
        await Task.Delay((int)noteCallback.notedelay + 3000);
        SongEndPanelController songEndPanel;
        songEndPanel = GameObject.Find("SongEndPanelController").GetComponent<SongEndPanelController>();
        songEndPanel.ToggleEndPanel();
    }

}
