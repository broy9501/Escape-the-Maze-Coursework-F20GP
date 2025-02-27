using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth; // Maximum health value of player
    public int currentHealth; // Current health value of player
    public HealthBar healthBar; // Reference health bar UI component
    public GameOver gameOverUI; // Reference to game over UI component

    // Track if player is currently invulnerable to damage and the duration of it in seconds after respawn
    public bool isInvulnerable = false;
    public float invulnerabilityDuration = 2.0f;

    void Start()
    {
        // Check if health bar is assigned and set maxHealt from health bar's maximum value and cuurent health to current slider value
        if (healthBar != null)
        {
            maxHealth = (int)healthBar.slider.maxValue;
            currentHealth = (int)healthBar.slider.value;
        }
    }

    // Method to handle damage taken by the player
    public void TakeDamage(int damage)
    {
        // Do nothing if invulnerable
        if (isInvulnerable)
            return;

        // Reduce current health by health damage amount and ensure health doesn't go below 0
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Update health bar UI if it exists
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        // Check if player has died
        if (currentHealth <= 0)
        {
            // Show game over screen if UI component exists, if not the show log warning
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOver();
            }
            else
            {
                Debug.LogWarning("No GameOver script assigned to PlayerHealth!");
            }
        }
    }

    // Method to reset player's health to maximum
    public void ResetHealth()
    {
        // Restore health to maximum and update the health bar UI
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        // Make the player invulnerable after respawn for the duration
        StartCoroutine(TemporaryInvulnerability());
    }

    // Coroutine for handling temporary invulnerability period
    private IEnumerator TemporaryInvulnerability()
    {
        // Player becomes invulnerability for the specific duration
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false; // Remove invulnerability after the duration
    }
}

/*
     * References:
     * https://www.youtube.com/watch?v=BLfNP4Sc_iA&list=LL&index=1&t=831s
     * https://docs.unity3d.com/Manual/coroutines.html
*/