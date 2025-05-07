using UnityEngine;

public class Handle2PositionLoader : MonoBehaviour
{
    public bool spawnWhenReady = true;

    void Start()
    {
        if (spawnWhenReady && Handle1PositionStorage.hasSavedPosition)
        {
            transform.position = Handle1PositionStorage.savedPosition;
            Debug.Log("Loaded saved position for Handle1.");
        }
    }
}