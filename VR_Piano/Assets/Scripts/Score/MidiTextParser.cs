using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MidiParser {
    public static List<NoteEvent> Parse(string[] lines, float ticksPerQuarterNote, float bpm, bool isLeftHand = false) {
        var events = new List<NoteEvent>();
        var noteOnTimes = new Dictionary<int, float>();
        float secondsPerTick = 60f / (bpm * ticksPerQuarterNote);

        foreach (var line in lines) {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(' ');
            if (parts.Length < 4) continue;

            string type = parts[0];
            int pitch = int.Parse(parts[1]);
            int velocity = int.Parse(parts[2]);
            int tick = int.Parse(parts[3]);

            float time = tick * secondsPerTick;

            if (type == "on") {
                noteOnTimes[pitch] = time;
            } else if (type == "off" && noteOnTimes.ContainsKey(pitch)) {
                float start = noteOnTimes[pitch];
                float duration = time - start;
                string symbol = SymbolMapper.MapNoteDurationToSymbol(duration, secondsPerTick * ticksPerQuarterNote);
                events.Add(new NoteEvent(pitch, start, duration, false, symbol, isLeftHand));
                noteOnTimes.Remove(pitch);
            }
        }
        return events;
    }
}
