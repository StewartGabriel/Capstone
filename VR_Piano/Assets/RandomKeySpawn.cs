using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomKeySpawn : MonoBehaviour
{
    public SpawnKeys spawnKeys;

    private int spawnCount = 8; // Number of times to spawn
    private float interval = 1.0f; // Interval in seconds between spawns

    private void Start()
    {
        // Start the spawn coroutine on start
        StartCoroutine(SpawnRandomKey());
    }

    private IEnumerator SpawnRandomKey()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // Call the SpawnRandomKey method from SpawnKeys
            spawnKeys.SpawnRandomKey();
            
            // Wait for the specified interval before the next spawn
            yield return new WaitForSeconds(interval);
        }
    }
}
