using UnityEngine;
using System.Collections;

// Class to handle the win condition when the player enters the trigger zone
public class WinTrigger : MonoBehaviour
{
    public Camera winCamera; // References to camera used for the win view
    public Camera playerCamera; // Reference to the player's main camera
    private Animator animatorPlayer; // Reference to player's animator component
    public AudioSource winMusic; // Reference to audio source for win music
    public AudioSource backgroundMusic; // Reference to audio source for background music

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Ensure player camera is enabled and win camera is disabled at the start
        if (playerCamera != null) playerCamera.enabled = true;
        if (winCamera != null) winCamera.enabled = false;
    }
    
    // Called when another collider enters thsi object's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        animatorPlayer = other.GetComponent<Animator>(); // Get Animator component from object that entered the trigger

        // Check if the object entering the trigger is tagged as "Player" 
        if (other.CompareTag("Player")) {
            if (animatorPlayer != null)
            {
                // If the player has an Animator, trigger the "Dance" animation and log it
                animatorPlayer.SetTrigger("Dance");
                Debug.Log("Now Dancing!");
            }
            else {
                Debug.Log("No animator found!");
            }

            // Stop the background music and play the winning music
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
            }

            if (winMusic != null) { 
                winMusic.Play();
            }

            // Start a coroutine to switch cameras after the trigger is activated
            StartCoroutine(SwitchToWinCamera());
        }
    }

    // Coroutine to handle camera switching and timing
    private IEnumerator SwitchToWinCamera()
    {
        // Check if both cameras are assigned, then switch from player to win camera
        if (playerCamera != null && winCamera != null)
        {
            playerCamera.enabled = false;
            winCamera.enabled = true;
        }
        else {
            Debug.Log("Cameras not assigned!");
        }

        // Wait 80 seconds before switching back
        yield return new WaitForSeconds(80.0f);

        // Switch back to the player camera after the wait
        if (playerCamera != null && winCamera != null) { 
            winCamera.enabled = false;
            playerCamera.enabled = true;
        }
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
     * https://docs.unity3d.com/ScriptReference/Animator.SetTrigger.html
     * https://docs.unity3d.com/ScriptReference/AudioSource.html
     * https://discussions.unity.com/t/changing-between-cameras/3254/4
*/
