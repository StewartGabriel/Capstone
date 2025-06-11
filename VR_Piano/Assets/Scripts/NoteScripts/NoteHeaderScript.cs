using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class NoteHeader : MonoBehaviour
{
    public Renderer thisnotesrenderer;
    private Transform parentTransform;
    private float targetZScale;
    public bool moving = true;
    public float deathdelay = 1f;
    public Material[] materials;

    void Start()
    {
        parentTransform = transform.parent;
        targetZScale = transform.lossyScale.z;
    }

    void Update()
    {
        Vector3 ps = parentTransform.lossyScale;
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            targetZScale / ps.z
        );
    }

    private void maintaincourse()
    {
            
    }

    public void Hit()
    {
        thisnotesrenderer.material = materials[1];
        StartCoroutine(DestroyAfterDelay());
    }

    public void Miss()
    {
        thisnotesrenderer.material = materials[2];
        StartCoroutine(DestroyAfterDelay());
    }
    
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathdelay);
        Destroy(gameObject);
    }
}
