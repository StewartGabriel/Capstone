using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLeader : MonoBehaviour
{
    public Note parentnote;
    public Material[] leadermaterials;
    public Renderer thisnotesrenderer;
    // Start is called before the first frame update
    void Start()
    {
        disappear();
    }

    // Update is called once per frame
    void Update()
    {
        float targetLength = (parentnote.endtime - Time.time);
        float globaltargetlength = targetLength / transform.parent.lossyScale.z;

        // Update local scale
        Vector3 localScale = transform.localScale;
        if(globaltargetlength < 1000)
        localScale.z = globaltargetlength;
        transform.localScale = localScale;

        // Use the same direction as the note movement
        Vector3 direction = parentnote.transform.forward.normalized;
        transform.position = parentnote.transform.position - direction * (targetLength / 2f - parentnote.transform.lossyScale.z / 2f);
    }
    public void appear()
    {
        thisnotesrenderer.enabled = true;
        thisnotesrenderer.material = leadermaterials[0];
    }
    public void disappear()
    {
        thisnotesrenderer.enabled = false;
        //thisnotesrenderer.material = leadermaterials[1]; 
    }
}
