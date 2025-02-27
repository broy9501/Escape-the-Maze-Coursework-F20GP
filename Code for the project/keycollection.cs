using UnityEngine;
using TMPro;
using System.Collections;

public class keycollection : MonoBehaviour
{
    private int key = 0;
    public TextMeshProUGUI keytext;
    public GameObject wallToLift; // The wall that lifts when a key is collected
    public GameObject wallToDestroy; // The wall (prevent the character from going through the door before key collection) to destory 
    public float moveHeight = 3f; // How high the wall moves
    public float moveSpeed = 2f;  // Speed of movement

    public Camera playerCamera; // Main player camera
    public Camera wallCamera;   // Extra camera to show the wall lifting

    private Vector3 wallStartPos;
    private bool shouldMove = false;

    private void Start()
    {
        // Store the initial position of the wall so it can move relative to this position
        if (wallToLift != null)
        {
            wallStartPos = wallToLift.transform.position;
        }

        // Ensure playerCamera is active and wallCamera is disabled at start
        if (playerCamera != null)
            playerCamera.enabled = true;
        if (wallCamera != null)
            wallCamera.enabled = false;
    }

    private void Update()
    {
        if (shouldMove && wallToLift != null)
        {
            // Move the wall smoothly upward using Lerp
            wallToLift.transform.position = Vector3.Lerp(
                wallToLift.transform.position,
                wallStartPos + Vector3.up * moveHeight,
                moveSpeed * Time.deltaTime
            );

            // Destroy another wall if needed
            if (wallToDestroy != null)
                Destroy(wallToDestroy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a key is collected (assuming the key's tag is "key")
        if (other.CompareTag("key"))
        {
            // Increment key count and update UI text
            key++;
            keytext.text = "Keys: " + key.ToString();
            Debug.Log("Key Collected: " + key);

            // Reset player's health to full by finding the PlayerHealth component on the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ResetHealth();
                    Debug.Log("Player health reset to full!");
                }
                else
                {
                    Debug.LogWarning("PlayerHealth component not found on the player!");
                }
            }
            else
            {
                Debug.LogWarning("Player not found in scene!");
            }

            // Remove the key from the scene
            Destroy(other.gameObject);

            // Start moving the wall (cutscene)
            shouldMove = true;

            // Switch cameras for a cutscene effect
            StartCoroutine(SwitchToWallCamera());
        }
    }

    IEnumerator SwitchToWallCamera()
    {
        // Switch to the wall camera view
        if (wallCamera != null && playerCamera != null)
        {
            playerCamera.enabled = false;
            wallCamera.enabled = true;
        }

        // Wait for 3 seconds so the cutscene is visible
        yield return new WaitForSeconds(2f);

        // Switch back to the player camera
        if (wallCamera != null && playerCamera != null)
        {
            wallCamera.enabled = false;
            playerCamera.enabled = true;
        }
    }
}

/*
     * References:
     * https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
     * https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
     * https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
*/
