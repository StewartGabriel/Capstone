using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanelScript : MonoBehaviour
{
    public ListeningBoard playerboard;
    public TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerboard.transform.position + playerboard.transform.forward * 1 + playerboard.transform.up * .5f;
        transform.rotation = playerboard.transform.rotation;
        ScoreText.text = "Correct Notes: " + playerboard.notemanager.correctnotes.ToString() + "\nIncorrect Notes: " + playerboard.notemanager.incorrectnotes;

    }
}
