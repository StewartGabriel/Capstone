using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class MidiMessages : MonoBehaviour
{
    private GameObject toRender; // ** line can be deleted after figuring out where to send the data in Unity 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame) // I'm guessing this will be replaced with Play > Song > from the menu
        {
            string filePath = Application.dataPath + "/processedFurElise1Piano RH.txt"; // if Play > Song condition then filePath should be adjusted to current file folder/selected song

            string fileName = Path.GetFileNameWithoutExtension(filePath); // Gets the file name only         
            string gameObjectName = "toRender: " + fileName; // To rename the created Game Object with the file name

            // Create temporary Game Object
            toRender = new GameObject(gameObjectName); // ** can be deleted

            // Auto-Generate ExtractedData component to store the parsed data
            toRender.AddComponent<ExtractedData>(); // ** can be deleted


            // Checks if the files exists
            if (File.Exists(filePath))
            {
                // Access the ExtractedData component
                ExtractedData extractedData = toRender.GetComponent<ExtractedData>(); // ** can be deleted

                // Clears ExtractedData before storing new data
                extractedData.oldData.Clear(); // ** can be deleted

                // Reads all the lines of the file
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] index = line.Split(' ');

                    if (index.Length > 0)
                    {
                        string onOff = index[0];
                        int noteNum = int.Parse(index[1]);
                        int velocity = int.Parse(index[2]);
                        int timeDelay = int.Parse(index[3]);

                        // Sends the extracted data to a designated Game Object in Unity
                        // GameObject toRender = GameObject.Find("NoteCallback");
                        /*
                         * Commented this part out for now because 
                         * I'm not sure where in Unity
                         * the data needs to be sent to
                         * */

                        // How the data is formatted
                        string newData = $"Note on/off: {onOff}, Midi Number: {noteNum}, Velocity: {velocity}, Time Delay: {timeDelay}";

                        // Store the data in ExtractedData component
                        extractedData.oldData.Add(newData); // ** can be deleted

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


// ** line can be deleted after figuring out where to send the data in Unity
public class ExtractedData : MonoBehaviour
{
    public System.Collections.Generic.List<string> oldData = new System.Collections.Generic.List<string>();
}
