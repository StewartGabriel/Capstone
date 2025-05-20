using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreRenderer : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform scoreArea;
    public GameObject notePrefab;
    public GameObject ledgerLinePrefab;
    public GameObject staffLinePrefab;
    public float defaultBPM = 120f;
    public float ticksPerQuarterNote = 480f;
    public TextAsset[] songFiles;

    private float songTime = 0f;
    private float secondsPerBeat;
    private float measureWidth = 400f;
    private const float staffSpacing = 100f;

    private List<NoteEvent> noteEvents = new();
    private List<GameObject> spawnedNotes = new();
    private Queue<GameObject> notePool = new();
    private int poolSize = 100;

    private AudioSource audioSource;
    private int slotsPerBeat = 4;
    private float slotDuration;
    private float totalSongDuration;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        int songIndex = PlayerPrefs.GetInt("SelectedSong", 1);
        bool leftEnabled = PlayerPrefs.GetInt("LeftEnabled", 1) == 1;
        bool rightEnabled = PlayerPrefs.GetInt("RightEnabled", 1) == 1;
        float tempoMultiplier = PlayerPrefs.GetFloat("TempoMultiplier", 1f);

        float bpm = defaultBPM * tempoMultiplier;
        secondsPerBeat = 60f / bpm;
        slotDuration = secondsPerBeat / slotsPerBeat;

        // Parse song files
        if (songIndex >= 1 && songIndex * 2 <= songFiles.Length)
        {
            if (leftEnabled)
            {
                TextAsset leftMidi = songFiles[(songIndex * 2) - 2];
                if (leftMidi != null)
                    noteEvents.AddRange(MidiParser.Parse(leftMidi.text.Split('\n'), ticksPerQuarterNote, bpm, true));
            }

            if (rightEnabled)
            {
                TextAsset rightMidi = songFiles[(songIndex * 2) - 1];
                if (rightMidi != null)
                    noteEvents.AddRange(MidiParser.Parse(rightMidi.text.Split('\n'), ticksPerQuarterNote, bpm, false));
            }
        }
        else
        {
            Debug.LogError("Invalid song index or missing files.");
            return;
        }

        noteEvents.Sort((a, b) => a.startTime.CompareTo(b.startTime));
        totalSongDuration = noteEvents.Max(n => n.startTime + n.duration);

        float totalWidth = GetTotalScoreWidth();
        scoreArea.sizeDelta = new Vector2(totalWidth, scoreArea.sizeDelta.y);

        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(notePrefab);
            obj.transform.SetParent(scoreArea, false);
            obj.SetActive(false);
            notePool.Enqueue(obj);
        }

        DrawStaffLines();
        RenderScoreWithRests();
        audioSource.Play();
    }

    void Update()
    {
        songTime += Time.deltaTime;

        float normalizedScroll = songTime / totalSongDuration;
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedScroll);

        foreach (var obj in spawnedNotes)
        {
            var display = obj.GetComponent<NoteDisplay>();
            var evt = obj.GetComponent<NoteComponent>().noteEvent;

            bool isPlaying = songTime >= evt.startTime && songTime < evt.startTime + evt.duration;
            display.Highlight(isPlaying);
        }
    }

    void RenderScoreWithRests()
    {
        int totalSlots = Mathf.CeilToInt(totalSongDuration / slotDuration);
        Dictionary<int, List<NoteEvent>> slotMap = new();

        foreach (var note in noteEvents)
        {
            int slotIdx = Mathf.FloorToInt(note.startTime / slotDuration);
            if (!slotMap.ContainsKey(slotIdx))
                slotMap[slotIdx] = new List<NoteEvent>();
            slotMap[slotIdx].Add(note);
        }

        for (int i = 0; i < totalSlots; i++)
        {
            float slotTime = i * slotDuration;

            if (slotMap.ContainsKey(i))
            {
                foreach (var note in slotMap[i])
                    SpawnNote(note);
            }
            else
            {
                var rest = new NoteEvent(
                    pitch: 0,
                    startTime: slotTime,
                    duration: slotDuration,
                    isRest: true,
                    symbol: SymbolMapper.MapRestDurationToSymbol(slotDuration, secondsPerBeat),
                    isLeftHand: false
                );
                SpawnNote(rest);
            }
        }
    }

    void SpawnNote(NoteEvent evt)
    {
        GameObject go = notePool.Count > 0 ? notePool.Dequeue() : Instantiate(notePrefab, scoreArea);
        go.transform.SetParent(scoreArea, false);
        go.SetActive(true);

        var noteComponent = go.GetComponent<NoteComponent>() ?? go.AddComponent<NoteComponent>();
        noteComponent.noteEvent = evt;

        var disp = go.GetComponent<NoteDisplay>();
        string symbol = evt.symbol ?? (
            evt.isRest
            ? SymbolMapper.MapRestDurationToSymbol(evt.duration, secondsPerBeat)
            : SymbolMapper.MapNoteDurationToSymbol(evt.duration, secondsPerBeat)
        );
        string pitchLetter = evt.isRest ? "" : SymbolMapper.MapPitchToLetter(evt.pitch);
        disp.SetNote(symbol, pitchLetter);

        // Accurate measure + beat-based X positioning
        float measureDuration = secondsPerBeat * 4f;
        int measureIndex = Mathf.FloorToInt(evt.startTime / measureDuration);
        float beatOffset = (evt.startTime % measureDuration) / secondsPerBeat;
        float x = (measureIndex * measureWidth) + (beatOffset * (measureWidth / 4f));

        float y = evt.isRest ? 0 : PitchToVerticalPosition(evt);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x, y);
        spawnedNotes.Add(go);

        if (!evt.isRest)
            DrawLedgerLinesIfNeeded(evt.pitch, x);
    }

    float GetTotalScoreWidth()
    {
        int totalMeasures = Mathf.CeilToInt(totalSongDuration / (secondsPerBeat * 4));
        return totalMeasures * measureWidth;
    }

    float PitchToVerticalPosition(NoteEvent evt)
    {
        float lineSpacing = 10f;
        int middlePitch = 60;
        float relativeY = (evt.pitch - middlePitch) * lineSpacing;
        return evt.isLeftHand ? -staffSpacing + relativeY : staffSpacing + relativeY;
    }

    void DrawLedgerLinesIfNeeded(int pitch, float xPos)
    {
        int middleC = 60;
        int lowestStaffNote = 52;
        int highestStaffNote = 76;

        if (pitch >= lowestStaffNote && pitch <= highestStaffNote)
            return;

        List<int> ledgerPitches = new();

        if (pitch < lowestStaffNote)
        {
            for (int p = pitch; p <= lowestStaffNote; p += 2)
                ledgerPitches.Add(p);
        }
        else if (pitch > highestStaffNote)
        {
            for (int p = pitch; p >= highestStaffNote; p -= 2)
                ledgerPitches.Add(p);
        }

        foreach (int ledgerPitch in ledgerPitches)
        {
            bool isLeft = pitch < middleC;
            var tempEvt = new NoteEvent(ledgerPitch, 0, 0, false, null, isLeft);
            float y = PitchToVerticalPosition(tempEvt);
            GameObject ledger = Instantiate(ledgerLinePrefab, scoreArea);
            RectTransform rt = ledger.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xPos, y);
            rt.SetAsFirstSibling();
        }
    }

    void DrawStaffLines()
    {
        float lineSpacing = 10f;

        for (int i = 0; i < 5; i++)
        {
            float y = staffSpacing + (i - 2) * lineSpacing;
            var line = Instantiate(staffLinePrefab, scoreArea);
            line.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            line.transform.SetAsFirstSibling();
        }

        for (int i = 0; i < 5; i++)
        {
            float y = -staffSpacing + (i - 2) * lineSpacing;
            var line = Instantiate(staffLinePrefab, scoreArea);
            line.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            line.transform.SetAsFirstSibling();
        }
    }
}