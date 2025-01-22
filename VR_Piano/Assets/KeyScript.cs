using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Renderer thisKeysRenderer;
    public Note NotePrefab; 
    public Material[] Materials;
    private Note currentnote;
    private bool isactive;
    // Start is called before the first frame update
    void Start()
    {
        thisKeysRenderer.material = Materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void KeyDown(int speed)
    {
        Vector3 spawnPosition = transform.position + transform.forward * (transform.localScale.z / 2);

        thisKeysRenderer.material = Materials[1];
        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 
    0f, 
    0f);
        Note newNote = Instantiate(
                NotePrefab,spawnPosition,
                Quaternion.identity
            );
        newNote.transform.SetParent(this.transform);
        currentnote = newNote;
        transform.Translate(Vector3.down * .05f, Space.Self);

    }
    public void KeyUp()
    {
        //Debug.Log("KeyUp called");
        thisKeysRenderer.material = Materials[0];
        currentnote.on = false;
        transform.Translate(Vector3.up * .05f, Space.Self);
    }
}
