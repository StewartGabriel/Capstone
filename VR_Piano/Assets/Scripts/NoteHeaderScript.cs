using UnityEngine;

public class KeepLossyScaleConstant : MonoBehaviour
{
    private Transform parentTransform;
    private float targetZScale; // The desired Z world scale

    void Start()
    {
        // Get the parent transform
        parentTransform = transform.parent;

        // Store the desired target Z scale based on the initial world scale
        targetZScale = transform.lossyScale.z;
    }

    void Update()
    {
        // Get the current parent's lossy scale
        Vector3 parentLossyScale = parentTransform.lossyScale;

        // Calculate the factor needed to maintain the global Z scale constant
        float scaleFactor = parentLossyScale.z / transform.localScale.z;

        // Adjust the child's local scale while maintaining the target global Z scale
        Vector3 newScale = new Vector3(
            transform.localScale.x, 
            transform.localScale.y, 
            targetZScale / parentLossyScale.z);

        transform.localScale = newScale;
    }
}
