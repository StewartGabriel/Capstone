using TMPro;
using UnityEngine;

public class ScoreNoteTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI noteText;
    public float staffSpacing = 30f;     // Pixels per note step (e.g., between lines)
    public float horizontalScale = 100f; // Pixels per second of time
    public float lifeTime = 10f;         // Destroy after this duration

    private float spawnTime;
    private RectTransform rectTransform;

    public void Setup(string noteSymbol, int midiNote, float timeOffset, bool isLeftHand)
    {
        if (noteText == null)
            noteText = GetComponent<TextMeshProUGUI>();

        noteText.text = noteSymbol;
        rectTransform = GetComponent<RectTransform>();

        Vector3 pos = GetNotePosition(midiNote, timeOffset, isLeftHand);
        rectTransform.anchoredPosition = new Vector2(pos.x, pos.y);

        spawnTime = Time.time;
    }

    private Vector3 GetNotePosition(int midiNote, float timeOffset, bool isLeftHand)
    {
        float y = (midiNote - 60) * staffSpacing;
        float x = timeOffset * horizontalScale;
        float z = isLeftHand ? -0.1f : 0.1f;
        return new Vector3(x, y, z);
    }

    void Update()
    {
        float elapsed = Time.time - spawnTime;
        rectTransform.anchoredPosition -= new Vector2(Time.deltaTime * horizontalScale, 0);

        if (elapsed > lifeTime)
            Destroy(gameObject);
    }

    public void Highlight()
    {
        noteText.color = Color.yellow;
        noteText.fontStyle = FontStyles.Bold;
    }

    public void Unhighlight()
    {
        noteText.color = Color.white;
        noteText.fontStyle = FontStyles.Normal;
    }
}
