using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreVisualizer : MonoBehaviour
{
    [Header("References")]
    public RectTransform staffArea;
    public TMP_FontAsset musisyncFont;
    public GameObject symbolTemplate;

    [Header("Layout Settings")]
    public float semitoneSpacing = 2.5f;
    public float horizontalSpacing = 30f;
    public float beatLength = 500f;
    public int middleC = 60;

    [Header("Bars Displayed")]
    public int beatsPerBar = 4;
    public int barsPerPage = 8;

    [Header("Symbol Map")]
    public List<SymbolDefinition> customSymbols; // Configurable in Inspector

    private Dictionary<string, string> symbolMap = new();

    private class VisualNote
    {
        public GameObject container;
        public TMP_Text symbol;
        public TMP_Text highlight;
        public int slotIndex;
    }

    private List<VisualNote> placedNotes = new();

    [System.Serializable]
    public class SymbolDefinition
    {
        public string name;
        public string character;
    }

    private void Awake()
    {
        symbolMap.Clear();
        foreach (var entry in customSymbols)
        {
            if (!string.IsNullOrWhiteSpace(entry.name) && !string.IsNullOrWhiteSpace(entry.character))
                symbolMap[entry.name] = entry.character;
        }
    }

    public void RenderScore(List<MidiScoreGenerator.MidiNote> notes)
    {
        placedNotes.Clear();
        if (notes == null || notes.Count == 0) return;

        int totalBeats = beatsPerBar * barsPerPage;

        // Group notes by beat slot
        Dictionary<int, List<MidiScoreGenerator.MidiNote>> slots = new();
        foreach (var note in notes)
        {
            int slotIndex = Mathf.FloorToInt(note.startTime / beatLength);
            if (slotIndex >= totalBeats) continue;

            if (!slots.ContainsKey(slotIndex))
                slots[slotIndex] = new List<MidiScoreGenerator.MidiNote>();

            slots[slotIndex].Add(note);
        }

        // Draw clefs once at the beginning of the system
        DrawClefs();

        // Draw notes/rests in beat slots
        for (int i = 0; i < totalBeats; i++)
        {
            float x = i * horizontalSpacing;

            if (slots.ContainsKey(i))
            {
                foreach (var note in slots[i])
                {
                    string symbol = GetNoteSymbol(note.duration);
                    float y = (note.noteNumber - middleC) * semitoneSpacing;
                    CreateMusicSymbol(symbol, x, y, i);
                }
            }
            else
            {
                string restSymbol = GetRestSymbol(beatLength);
                CreateMusicSymbol(restSymbol, x, 0f, i);
            }
        }

        RenderBarlines();
    }

    private void DrawClefs()
    {
        float clefX = -horizontalSpacing;

        // Treble clef (top staff)
        if (symbolMap.TryGetValue("clef_treble", out var trebleClef))
        {
            float yTreble = 6 * semitoneSpacing;
            CreateMusicSymbol(trebleClef, clefX, yTreble, -1);
        }

        // Bass clef (bottom staff)
        if (symbolMap.TryGetValue("clef_bass", out var bassClef))
        {
            float yBass = -2 * semitoneSpacing;
            CreateMusicSymbol(bassClef, clefX, yBass, -1);
        }
    }

    private void CreateMusicSymbol(string symbol, float x, float y, int slotIndex)
    {
        GameObject go = Instantiate(symbolTemplate, staffArea);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x, y);

        TMP_Text[] texts = go.GetComponentsInChildren<TMP_Text>();
        TMP_Text highlight = null;
        TMP_Text symbolText = null;

        foreach (var t in texts)
        {
            if (t.name.ToLower().Contains("highlight")) highlight = t;
            else symbolText = t;
        }

        if (highlight == null || symbolText == null)
        {
            Debug.LogError("Symbol prefab must contain 'Highlight' and 'Symbol' TMP_Text objects.");
            return;
        }

        foreach (var t in new[] { highlight, symbolText })
        {
            t.text = symbol;
            t.font = musisyncFont;
            t.alignment = TextAlignmentOptions.Center;
        }

        highlight.fontSize = symbolText.fontSize + 10;
        highlight.color = new Color(1f, 0.9f, 0.3f);
        highlight.enabled = false;

        symbolText.color = Color.black;

        placedNotes.Add(new VisualNote
        {
            container = go,
            symbol = symbolText,
            highlight = highlight,
            slotIndex = slotIndex
        });
    }

    public void HighlightCurrentSlot(int currentSlot)
    {
        foreach (var note in placedNotes)
        {
            note.highlight.enabled = (note.slotIndex == currentSlot);
        }
    }

    private string GetNoteSymbol(float duration)
    {
        if (duration >= 2000f && symbolMap.TryGetValue("note_whole", out var whole)) return whole;
        if (duration >= 1000f && symbolMap.TryGetValue("note_half", out var half)) return half;
        if (duration >= 500f && symbolMap.TryGetValue("note_quarter", out var quarter)) return quarter;
        if (symbolMap.TryGetValue("note_eighth", out var eighth)) return eighth;

        return "?";
    }

    private string GetRestSymbol(float duration)
    {
        if (duration >= 2000f && symbolMap.TryGetValue("rest_whole", out var whole)) return whole;
        if (duration >= 1000f && symbolMap.TryGetValue("rest_half", out var half)) return half;
        if (duration >= 500f && symbolMap.TryGetValue("rest_quarter", out var quarter)) return quarter;
        if (symbolMap.TryGetValue("rest_eighth", out var eighth)) return eighth;

        return "-";
    }

    private void RenderBarlines()
    {
        for (int i = 1; i < barsPerPage; i++)
        {
            float x = i * beatsPerBar * horizontalSpacing;

            GameObject bar = Instantiate(symbolTemplate, staffArea);
            RectTransform rt = bar.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(x, 0f);

            TMP_Text[] texts = bar.GetComponentsInChildren<TMP_Text>();
            foreach (var t in texts)
            {
                t.text = symbolMap.TryGetValue("barline", out var barChar) ? barChar : "|";
                t.font = musisyncFont;
                t.color = Color.black;
                t.fontSize = 60;
                t.alignment = TextAlignmentOptions.Center;
            }
        }
    }

    public void UpdatePlaybackHighlight(float currentTimeMs)
    {
        int currentSlot = Mathf.FloorToInt(currentTimeMs / beatLength);
        HighlightCurrentSlot(currentSlot);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-Populate Custom Symbols")]
    private void AutoPopulateCustomSymbols()
    {
        customSymbols = new List<SymbolDefinition>
        {
            new() { name = "note_whole", character = "𝅝" },
            new() { name = "note_half", character = "𝅗𝅥" },
            new() { name = "note_quarter", character = "♩" },
            new() { name = "note_eighth", character = "♪" },

            new() { name = "rest_whole", character = "𝄻" },
            new() { name = "rest_half", character = "𝄼" },
            new() { name = "rest_quarter", character = "𝄽" },
            new() { name = "rest_eighth", character = "𝄾" },

            new() { name = "clef_treble", character = "𝄞" },
            new() { name = "clef_bass", character = "𝄢" },

            new() { name = "barline", character = "|" },
            new() { name = "double_barline", character = "‖" },

            new() { name = "sharp", character = "#" },
            new() { name = "flat", character = "♭" },
            new() { name = "natural", character = "♮" }
        };

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
