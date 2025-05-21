using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLabelScript : MonoBehaviour
{
   private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void LateUpdate()
    {
        Vector3 parentScale = transform.parent.lossyScale;
        transform.localScale = new Vector3(
            (originalScale.x / parentScale.x)/60,
            (originalScale.y / parentScale.y)/260,
            (originalScale.z / parentScale.z)/100
        );
    }
}
