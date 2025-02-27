using UnityEngine;

public class KeySpawner : MonoBehaviour
{

    public GameObject key; // Reference to the key prefab that will be spawned
    public Transform[] spawnPoints; // Array of Transform objects representing possible spawn points for the key.
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Finds GameObject tagged with "key"
        GameObject existingKey = GameObject.FindGameObjectWithTag("key");

        // If an existing key is found, destroy it to ensure only one key exists at a time.
        if (existingKey != null)
        {
            Destroy(existingKey);
        }

        // Calls the SpawnKey method to create a new key at a random spawn point.
        SpawnKey();
    }


    // Method to spawn key each time game restarts in random loactions
    void SpawnKey()
    {
        // Checks if the key or spawnPoints array is unassigned or empty.
        if (key == null || spawnPoints.Length == 0) {
            Debug.Log("key or spawn points not set!");
            return;
        }

        // Generates a random index within the bounds of the spawnPoints array and picks a number from 0 to the length of spawnPoints
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenIndex = spawnPoints[randomIndex];

        // Creates a new instance of the key at the chosen position and rotation.
        GameObject keyInstance = Instantiate(key, chosenIndex.position, chosenIndex.rotation);
        // Assigns the "key" tag to the newly spawned key
        keyInstance.tag = "key";
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Object.Destroy.html
     * https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
     * https://docs.unity3d.com/ScriptReference/Random.Range.html
*/
