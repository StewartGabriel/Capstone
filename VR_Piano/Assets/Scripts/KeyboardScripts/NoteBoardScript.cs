using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

public class NoteBoard : MonoBehaviour
{
    public float thiccness;
    public OctaveFret OctaveFretPrefab;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildBoard(float width, float depth)
    {
        Vector3 newscale = transform.localScale;
        newscale.x = width;
        newscale.z = depth;
        newscale.y = thiccness;
        transform.localScale = newscale;
    }
    public void BuildFrets(List<float> fretlocation, float spacing)
    {
        foreach (float f in fretlocation)
        {
            Vector3 fretposition = transform.position +
            new Vector3(f - transform.lossyScale.x/2, transform.lossyScale.y / 2 + OctaveFretPrefab.transform.lossyScale.y/2, 0);
            //It's shifted too far over in the x, not sure why

            // Instantiate a new OctaveFret, not reusing OctaveFretPrefab
            OctaveFret newFret = Instantiate(OctaveFretPrefab, fretposition, Quaternion.identity);
            newFret.shapefret(spacing, transform.lossyScale.z);
            newFret.transform.parent = this.transform;
        }
    }

}
