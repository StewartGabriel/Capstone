using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public float notebuffer = .1f;
    public float notedelay = 0;
    public List<Note> activenotes  = new List<Note>();
    public List<Note> pressednotes = new List<Note>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Note i in activenotes){
            if(i.endtime < Time.time + notebuffer){
                Debug.Log("Deactivating note: " + i.noteID +", "+ i.endtime);
                i.deactivate();
            }
        }
    }

}
