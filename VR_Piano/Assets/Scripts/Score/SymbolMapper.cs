using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SymbolMapper
{
    public static string MapNoteDurationToSymbol(float durationInSeconds, float secondsPerBeat)
    {
        float beats = durationInSeconds / secondsPerBeat;

        if (beats >= 4f) return "\uE1D2"; // whole note
        else if (beats >= 2f) return "\uE1D3"; // half note
        else if (beats >= 1f) return "\uE1D5"; // quarter note
        else if (beats >= 0.5f) return "\uE1D7"; // eighth note
        else return "\uE1D9"; // sixteenth note
    }

    public static string MapRestDurationToSymbol(float durationInSeconds, float secondsPerBeat)
    {
        float beats = durationInSeconds / secondsPerBeat;

        if (beats >= 4f) return "\uE4E3"; // whole rest
        else if (beats >= 2f) return "\uE4E4"; // half rest
        else if (beats >= 1f) return "\uE4E5"; // quarter rest
        else if (beats >= 0.5f) return "\uE4E6"; // eighth rest
        else return "\uE4E7"; // sixteenth rest
    }

    public static string MapPitchToLetter(int pitch)
    {
        return (pitch % 12) switch
        {
            0 => "C",
            1 => "C#",
            2 => "D",
            3 => "D#",
            4 => "E",
            5 => "F",
            6 => "F#",
            7 => "G",
            8 => "G#",
            9 => "A",
            10 => "A#",
            11 => "B",
            _ => "?"
        };
    }
}
