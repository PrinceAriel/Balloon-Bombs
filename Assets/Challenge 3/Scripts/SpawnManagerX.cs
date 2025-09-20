using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private float spawnDelay = 2;
    private float spawnInterval = 1.5f;

    private PlayerControllerX playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player controller
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerX>();
        
        // Check if we found the player and have prefabs assigned
        if (playerControllerScript == null)
        {
            Debug.LogError("SpawnManagerX: Could not find Player with PlayerControllerX component!");
            return;
        }
        
        if (objectPrefabs.Length == 0)
        {
            Debug.LogError("SpawnManagerX: No object prefabs assigned in inspector!");
            return;
        }
        
        // Start spawning objects
        InvokeRepeating("SpawnObjects", spawnDelay, spawnInterval);
        Debug.Log("SpawnManagerX: Started spawning objects every " + spawnInterval + " seconds");
    }

    // Spawn obstacles
    void SpawnObjects()
    {
        // Check if game is still active and we have prefabs
        if (playerControllerScript != null && !playerControllerScript.gameOver && objectPrefabs.Length > 0)
        {
            // Set random spawn location (right side of screen, random height)
            Vector3 spawnLocation = new Vector3(25, Random.Range(5, 15), 0);
            int index = Random.Range(0, objectPrefabs.Length);

            // Make sure the selected prefab exists
            if (objectPrefabs[index] != null)
            {
                GameObject spawnedObject = Instantiate(objectPrefabs[index], spawnLocation, objectPrefabs[index].transform.rotation);
                Debug.Log("SpawnManagerX: Spawned " + objectPrefabs[index].name + " at " + spawnLocation);
            }
            else
            {
                Debug.LogError("SpawnManagerX: objectPrefabs[" + index + "] is null!");
            }
        }
    }
}