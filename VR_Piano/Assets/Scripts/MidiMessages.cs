using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System.Threading;
using System.Threading.Tasks;

public class MidiMessages : MonoBehaviour
{
    // private GameObject toRender; // ** line can be deleted after figuring out where to send the data in Unity

    public TalkingBoard toNoteCallback; // create reference to NoteCallback

    // Start is called before the first frame update
    void Start()
    {
        if (toNoteCallback == null)
        {
            Debug.LogError("NoteCallback not referenced correctly");
        }
    }

    // Update is called once per frame
    async void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame) // I'm guessing this will be replaced with Play > Song > from the menu
        { // if Play > Song condition then filePath should be adjusted to current file folder/selected song
            string filePath = "Sounds/Midi Files/FurElise/processedFurElise1Piano right";
            TextAsset midiMessages = Resources.Load<TextAsset>(filePath);
            Debug.Log(midiMessages.text);

            // Assets/Sounds/Midi Files/FurElise/processedFurElise1Piano right.txt

            //string fileName = Path.GetFileNameWithoutExtension(filePath); // Gets the file name only         
            //string gameObjectName = "toRender: " + fileName; // To rename the created Game Object with the file name

            //// Create temporary Game Object
            //toRender = new GameObject(gameObjectName); // ** can be deleted

            //// Auto-Generate ExtractedData component to store the parsed data
            //toRender.AddComponent<ExtractedData>(); // ** can be deleted


            // Checks if the files exists
            if (midiMessages != null)
            {
                //// Access the ExtractedData component
                //ExtractedData extractedData = toRender.GetComponent<ExtractedData>(); // ** can be deleted

                //// Clears ExtractedData before storing new data
                //extractedData.oldData.Clear(); // ** can be deleted

                // Reads all the lines of the file
                string[] lines = midiMessages.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

                foreach (string line in lines)
                {
                    string[] index = line.Split(' ');

                    if (index.Length > 0)
                    {
                        string onOff = index[0];
                        int note = int.Parse(index[1]);
                        int velocity = int.Parse(index[2]);
                        int timeDelay = int.Parse(index[3]);
                        // Thread.Sleep(timeDelay);
                        await Task.Delay(timeDelay);


                        // Sends the extracted data to NoteCallback component
                        if (toNoteCallback != null)
                        {
                            if (onOff == "on")
                            {
                                await Task.Delay(timeDelay);
                                toNoteCallback.InterpretMidi(note, velocity); // KeyDown
                            }

                            else
                            {
                                await Task.Delay(timeDelay);
                                toNoteCallback.InterpretMidi(note, 0); // KeyUp
                            }
                        }

                        // How the data is formatted
                        string newData = $"Note on/off: {onOff}, Midi Number: {note}, Velocity: {velocity}, Time Delay: {timeDelay}";

                        //// Store the data in ExtractedData component
                        //extractedData.oldData.Add(newData); // ** can be deleted

                        // Log Extracted data as Debug messages
                        Debug.Log(newData);
                    }
                }
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }
        }

    }
}


//// ** line can be deleted after figuring out where to send the data in Unity
//public class ExtractedData : MonoBehaviour
//{
//    public System.Collections.Generic.List<string> oldData = new System.Collections.Generic.List<string>();
//}
