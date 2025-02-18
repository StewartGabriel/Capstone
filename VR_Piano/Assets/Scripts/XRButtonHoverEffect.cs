using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class XRButtonHoverEffect : MonoBehaviour
{
    private Button button;
    private Color originalColor;
    public Color highlightColor = Color.yellow; // Set highlight color in Inspector

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            originalColor = button.colors.normalColor;
        }
        else
        {
            Debug.LogWarning($"XRButtonHoverEffect: No Button component found on {gameObject.name}");
        }
    }

   
    // Called when the Near-Far or Pinch Interactor hovers over the button.
    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        ChangeButtonColor(highlightColor);
    }


    // Called when the Near-Far or Pinch Interactor stops hovering over the button.
    public void OnHoverExit(HoverExitEventArgs args)
    {
        ChangeButtonColor(originalColor);
    }

    private void ChangeButtonColor(Color newColor)
    {
        if (button != null)
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = newColor;
            button.colors = colorBlock;
        }
    }
}
