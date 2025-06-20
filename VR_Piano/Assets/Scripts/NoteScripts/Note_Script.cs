using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Note : MonoBehaviour
{
    public float starttime;
    public float endtime;
    public float lifeTime; // Time after which the note gets destroyed
    public Material[] notematerials;
    public Renderer thisnotesrenderer;
    public float growRate; // Rate at which the object moves
    public bool on = true;
    public int noteID;

    private float timer = 0f;
    //private Transform parentTransform;
    private Vector3 movementdirection;
    public NoteLeader Leader;
    public NoteHeader Header;

    void Start()
    {
        Transform parentTransform = transform.parent; // Get the parent object
        movementdirection = parentTransform.forward;
        transform.rotation = parentTransform.rotation;
        Vector3 newscale = new Vector3(parentTransform.lossyScale.x, parentTransform.lossyScale.x, .01f);
        transform.SetParent(null);
        transform.localScale = newscale;
        thisnotesrenderer.material = notematerials[0];
        on = true;
    }

    void Update()
    {
        float growthAmount = growRate * Time.deltaTime;

        if (on)
        {
            transform.position -= movementdirection * growthAmount;
            transform.localScale += new Vector3(0, 0, growthAmount * 2);

        }
        else // If note is "off"
        {
            transform.position -= movementdirection * growthAmount * 2;

            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
    public void activate()
    {
        thisnotesrenderer.material = notematerials[1];
        Header.transform.parent = null;
    }

    public void activateleader()
    {
        Leader.appear();
    }
    public void deactivateleader()
    {
        Leader.disappear();
    }
    public void correct()
    {
        thisnotesrenderer.material = notematerials[3];
        Header.Hit();
    }
    public void incorrect()
    {
        if (thisnotesrenderer != null)
        {
            thisnotesrenderer.material = notematerials[2];
            deactivateleader();
        }
        Header.Miss();
    }
}
