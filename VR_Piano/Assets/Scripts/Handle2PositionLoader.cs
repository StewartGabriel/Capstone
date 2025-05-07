using UnityEngine;

public class Handle2PositionLoader : MonoBehaviour
{
    public bool spawnWhenReady = true;

    void Start()
    {
        if (spawnWhenReady && Handle2PositionStorage.hasSavedPosition)
        {
            transform.position = Handle2PositionStorage.savedPosition;
            Debug.Log("Loaded saved position for Handle2.");
        }
    }
}