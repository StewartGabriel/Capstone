using UnityEngine;

public class TestSound : MonoBehaviour
{
    public void OnChange()
    {
        SoundManager.PlaySound(SoundType.F2);
    }
}
