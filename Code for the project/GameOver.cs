using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public RawImage gameOverImage; // Reference the RawImage UI that displays the "Game Over" image
    public GameObject player;      // Reference the player
    public Transform respawn;      // Reference the respawn

    // Method to trigger the "Game Over"
    public void ShowGameOver()
    {
        // Display the game over image
        gameOverImage.gameObject.SetActive(true); // Activates the GameOver RawImage to make it visible to the player

        // Start the coroutine that waits 3 seconds before resetting the game
        StartCoroutine(WaitAndReset());
    }

    // Coroutine to handle waiting and resetting the game state
    private IEnumerator WaitAndReset()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1.0f);

        // Hide the game over image
        gameOverImage.gameObject.SetActive(false);

        // Reset the player's position to the respawn point
        if (player != null && respawn != null)
        {
            player.transform.position = respawn.position;

            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }
        }
    }
}

/*
     * References:
     * https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-RawImage.html
     * https://docs.unity3d.com/ScriptReference/Transform-position.html
     * https://learn.unity.com/tutorial/ui-components
*/
