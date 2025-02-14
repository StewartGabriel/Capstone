using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKeys : MonoBehaviour
{
    public GameObject Cube; //Key object 
    public Transform Plane; //Plane to spawn keys onto
    public float spawnHeight = 0.01f; // Height above the plane to spawn keys to be played
    public float planeWidth = 0.76f;  // Width of the plane

    private int totalPositions = 76;

    //spawn key at specified position index
    public void SpawnKeyAtPosition(int positionIndex)
    {
        if (positionIndex < 0 || positionIndex >= totalPositions)
        {
            Debug.LogError("Invalid position index. Must be between 0 and 75.");
            return;
        }

        // Calculate the spawn position along the width of the plane based on the input index
        float step = planeWidth / (totalPositions - 1);
        Vector3 spawnPosition = Plane.position + Plane.right * (positionIndex * step - planeWidth / 2) + Plane.up * spawnHeight;

        // Spawn the Key and set its name to the spawn position 
        GameObject spawnedKey = Instantiate(Cube, spawnPosition, Quaternion.identity);
        spawnedKey.name = $"Position_{positionIndex}";
    }

    // /* Random spawning for testing
    public void SpawnRandomKey()
    {
        int randomIndex = Random.Range(0, totalPositions);
        SpawnKeyAtPosition(randomIndex);
    }
    // */

}
