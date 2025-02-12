using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics; // For complex numbers

public class PianoKeyboardSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject pianoPrefab; // Prefab for the keyboard

    [SerializeField]
    private Transform leftHandIndexFingerTransform; // Reference to the left hand's index finger

    [SerializeField]
    private Transform rightHandIndexFingerTransform; // Reference to the right hand's index finger

    [SerializeField]
    private float keyPressThreshold = 0.02f; // Minimum finger movement to count as a key press

    [SerializeField]
    private int sampleRate = 44100; // Standard audio sample rate

    [SerializeField]
    private int fftSize = 1024; // Number of samples for FFT

    private AudioClip microphoneInput; //microphone inputted sound data in real time
    private float[] audioSamples; // 
    private float[] frequencyData; // 

    private GameObject pianoInstance;

    private bool isLeftAnchorSet = false;
    private bool isSettingAnchors = false; // Control when anchors can be set

    private UnityEngine.Vector3 leftAnchorPosition;
    private UnityEngine.Vector3 rightAnchorPosition;

    private UnityEngine.Vector3 leftFingerStartPos;
    private UnityEngine.Vector3 rightFingerStartPos;

    private bool isLeftFingerMoving = false;
    private bool isRightFingerMoving = false;

    void Start()
    {
        // Initialize audio arrays
        audioSamples = new float[fftSize];
        frequencyData = new float[fftSize / 2];

        // Start capturing microphone input
        StartMicrophone();
    }

    void Update()
    {
        if (!isSettingAnchors) return; // Do nothing if not in anchor-setting mode

        // Analyze audio input to detect keypress sounds
        if (DetectKeyPress(out float detectedFrequency))
        {
            Debug.Log($"Detected Frequency: {detectedFrequency} Hz");

            if (!isLeftAnchorSet)
            {
                // Map the detected frequency to the left anchor
                leftAnchorPosition = MapFrequencyToPosition(detectedFrequency);
                isLeftAnchorSet = true;
                Debug.Log("Left anchor set via sound: " + leftAnchorPosition);
            }
            else
            {
                // Map the detected frequency to the right anchor
                rightAnchorPosition = MapFrequencyToPosition(detectedFrequency);
                Debug.Log("Right anchor set via sound: " + rightAnchorPosition);
                PlacePianoBetweenAnchors();
                isSettingAnchors = false; // Exit anchor-setting mode
            }
        }

        // Check if the left anchor is set by detecting finger movement and position
        if (!isLeftAnchorSet && IsKeyPressed(leftHandIndexFingerTransform, ref isLeftFingerMoving, ref leftFingerStartPos, out UnityEngine.Vector3 leftKeyPosition))
        {
            leftAnchorPosition = leftKeyPosition;
            isLeftAnchorSet = true;
            Debug.Log("Left anchor set at: " + leftAnchorPosition);
        }
        // Check if the right anchor is set by detecting finger movement and position
        else if (isLeftAnchorSet && IsKeyPressed(rightHandIndexFingerTransform, ref isRightFingerMoving, ref rightFingerStartPos, out UnityEngine.Vector3 rightKeyPosition))
        {
            rightAnchorPosition = rightKeyPosition;
            Debug.Log("Right anchor set at: " + rightAnchorPosition);
            PlacePianoBetweenAnchors();
            isSettingAnchors = false; // Exit anchor-setting mode
        }
    }

    // Method to be called when setting the anchors for the piano keyboard
    public void EnableAnchorSetting()
    {
        isSettingAnchors = true;
        isLeftAnchorSet = false;
        Debug.Log("Anchor setting enabled. Waiting for input...");
    }

    // Method to be called to begin using the headset's microphone to record and recognize piano notes
    private void StartMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneInput = Microphone.Start(null, true, 1, sampleRate);
            Debug.Log("Microphone started.");
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    // Method to be called to detect wheter or not a key is pressed, and which key is pressed based on audio data
    private bool DetectKeyPress(out float detectedFrequency)
    {
        detectedFrequency = 0;

        if (microphoneInput == null) return false;

        // Get audio data from the microphone
        microphoneInput.GetData(audioSamples, 0);

        // Perform FFT using Math.NET
        FFTProcessor.FFT(audioSamples, frequencyData, sampleRate);

        // Find the dominant frequency
        int maxIndex = 0;
        float maxAmplitude = 0;
        for (int i = 0; i < frequencyData.Length; i++)
        {
            if (frequencyData[i] > maxAmplitude)
            {
                maxAmplitude = frequencyData[i];
                maxIndex = i;
            }
        }

        // Convert index to frequency
        detectedFrequency = FFTProcessor.GetFrequencyFromIndex(maxIndex, sampleRate, fftSize);

        // Check if the amplitude exceeds a threshold (indicating a key press)
        return maxAmplitude > 0.1f; // Adjust threshold as needed
    }

    // Method to take the played note's soundwave frequency and set the position of the key being pressed
    private UnityEngine.Vector3 MapFrequencyToPosition(float frequency)
    {
        // Map piano note frequencies to positions on the keyboard
        // Example: Middle C (261.63 Hz) is at x = 0
        float xPosition = (frequency - 261.63f) * 0.01f; // Adjust scaling as needed
        return new UnityEngine.Vector3(xPosition, 0, 0);
    }

    // Method to set the positions of the keys based on only hand movement (index finger tips)
    private bool IsKeyPressed(Transform fingerTransform, ref bool isFingerMoving, ref UnityEngine.Vector3 fingerStartPos, out UnityEngine.Vector3 keyPosition)
    {
        keyPosition = fingerTransform.position;

        // If the finger is not moving, set the start position
        if (!isFingerMoving)
        {
            fingerStartPos = fingerTransform.position;
            isFingerMoving = true;
            return false;
        }

        // Check if the finger has moved downward sufficiently to count as a key press
        float distanceMoved = Mathf.Abs(fingerTransform.position.y - fingerStartPos.y);
        if (distanceMoved > keyPressThreshold)
        {
            isFingerMoving = false; // Reset movement tracking
            return true;
        }

        return false;
    }

    // Method to anchor the piano between the two positions determined by either audio or hand tracking
    private void PlacePianoBetweenAnchors()
    {
        if (isLeftAnchorSet)
        {
            // Calculate the midpoint between the anchors
            UnityEngine.Vector3 midpoint = (leftAnchorPosition + rightAnchorPosition) / 2;

            // Calculate the distance between the anchors (keyboard length)
            float distance = UnityEngine.Vector3.Distance(leftAnchorPosition, rightAnchorPosition);

            // Instantiate the piano if not already placed
            if (pianoInstance == null)
            {
                pianoInstance = Instantiate(pianoPrefab, midpoint, UnityEngine.Quaternion.identity);
            }

            // Scale and position the piano
            pianoInstance.transform.position = midpoint;

            // Set the length of the keyboard by scaling its X-axis
            pianoInstance.transform.localScale = new UnityEngine.Vector3(distance, pianoInstance.transform.localScale.y, pianoInstance.transform.localScale.z);

            Debug.Log($"Piano placed! Length: {distance}");
        }
    }

    // Method to reset the piano position to 0,0 for simpler re-positioning 
    public void ResetPiano()
    {
        isLeftAnchorSet = false;
        leftAnchorPosition = UnityEngine.Vector3.zero;
        rightAnchorPosition = UnityEngine.Vector3.zero;

        if (pianoInstance != null)
        {
            Destroy(pianoInstance);
        }

        Debug.Log("Piano and anchors reset.");
    }
}