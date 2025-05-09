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

    // public async void PlaySong(int songIndex, bool left_enabled, bool right_enabled, int tempo_multiplier = 1)
    public async void PlaySong(int songIndex)
    {
        // Checks that song index is in range
        if (songIndex >= 1 && songIndex * 2 <= songFiles.Length)
        {
            // Getter for TextAsset according to songIndex
            TextAsset leftPart = songFiles[(songIndex * 2) - 2]; // Left part, even index
            TextAsset rightPart = songFiles[(songIndex * 2) - 1]; // Right part, odd index


            // if (left_enabled && leftPart != null){
            //     Task leftTask = ExtractMidiData(leftPart, true, tempo_multiplier);
            // }
            //
            // if (right_enabled && rightPart != null){
            //     Task rightTask = ExtractMidiData(rightPart, false, tempo_multiplier);
            // }
            //
            // await Task.WhenAll(leftTask, rightTask); <------- assuming this starts both tasks
            // else {
            //     Debug.LogError("TextAsset not found in index: " + songIndex);
            // }




            // Checks if the files exists
            if (leftPart != null && rightPart != null)
            {

                Debug.Log($"Playing song: {leftPart.name} (Left) and {rightPart.name} (Right)");

                Task leftTask = ExtractMidiData(leftPart, true);
                Task rightTask = ExtractMidiData(rightPart, false);

                await Task.WhenAll(leftTask, rightTask); // waits for both tasks to finish before moving to the next step
            }
            else
            {
                Debug.LogError("TextAsset not found in index: " + songIndex);
            }
        }
        else
        {
            Debug.LogError("songIndex out of range: " + songIndex);
        }
    }
    // public async Task ExtractMidiData(TextAsset midiMessages, bool hand, int tempo_multiplier)
    public async Task ExtractMidiData(TextAsset midiMessages,bool hand)
    {
        // Reads all the lines of the file
        string[] lines = midiMessages.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        int i = 0;
        foreach (string line in lines)
        {
            i++;
            string[] index = line.Split(' ');

            if (index.Length > 0)
            {
                string onOff = index[0];
                int note = int.Parse(index[1]);
                int velocity = int.Parse(index[2]);
                int timeDelay = int.Parse(index[3]);

                // Thread.Sleep(timeDelay);
                await Task.Delay(timeDelay);
                // await Task.Delay(timeDelay * 1/tempo_multiplier); 

                // Sends the extracted data to NoteCallback component
                if (toNoteCallback != null)
                {
                    if (onOff == "on")
                    {
                        await Task.Delay(timeDelay);
                        toNoteCallback.InterpretMidi(note, velocity,hand); // KeyDown
                    }

                    else
                    {
                        await Task.Delay(timeDelay);
                        toNoteCallback.InterpretMidi(note, 0, hand); // KeyUp
                    }
                }

                // Debug Log string format
                string debugData = $"Note on/off: {onOff}, Midi Number: {note}, Velocity: {velocity}, Time Delay: {timeDelay}, Line: {i}";

                Debug.Log(debugData);
            }
        }
    }

}
