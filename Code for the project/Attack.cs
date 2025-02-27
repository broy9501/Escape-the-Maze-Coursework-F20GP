using UnityEngine;
using UnityEngine.SceneManagement;

// Behaviour attack state in a Animator state machine for the enemy NPC
public class Attack : StateMachineBehaviour
{
    private GameObject player; // Reference to player
    private GameObject respawn; // Reference to spawn where player respawns in
    private GameObject bloodOverlay; // Reference to UI overlay for blood effect
    private BloodEffect bloodEffect; // Component handling blood visual effect

    public float damageInterval = 1.0f; // Time between damage applications
    private float lastDamageTime = 0f; // Tracks the last time damage was applied
    private int damageAmount = 5; // Amount of damage dealth per attack

    private float bloodEffectInterval = 1.0f; // Time between blood effect triggers
    private float lastBloodEffectTime; // Tracks the last time blood effect was shown

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find and assign references to key objects in the scene by their tags or names
        player = GameObject.FindGameObjectWithTag("Player");
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        bloodOverlay = GameObject.Find("BloodOverlay");

        // Checking for player, respawn, and blood overlay object
        if (respawn == null) {
            Debug.LogError("Player not found!");
            return;
        }

        if (respawn == null)
        {
            Debug.LogError("Respawn not found!");
            return;
        }

        if (bloodOverlay == null)
        {
            Debug.LogError("blood overlay not found!");
            return;
        }

        // Get the BloodEffect component from the blood overlay
        if (bloodOverlay != null)
        {
            bloodEffect = bloodOverlay.GetComponent<BloodEffect>();
        }

        // Initialize timing variables with current time and log the start of an attack
        lastDamageTime = Time.time;
        lastBloodEffectTime = Time.time;
        Debug.Log("Enemy attacking!");
        
        // Show blood effect if the component is available
        if (bloodEffect != null) {
            bloodEffect.ShowBloodEffect();
        }

        // Apply initial damage when entering the attack state
        ApplyDamage();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Calculate distance between player and the attacking enemy
        float currentDistance = Vector3.Distance(player.transform.position, animator.gameObject.transform.position);

        // If player moves too far (beyond 2.5 units), switch to chase state
        if (currentDistance > 2.5f)
        {
            animator.SetTrigger("Chase");
            return;
        }

        // Apply damage if enough time has passed since the last damage
        if (Time.time - lastDamageTime >= damageInterval) { 
            ApplyDamage();
            lastDamageTime = Time.time; // Reset the damage timer
        }

        // Show blood effect if enough time has passed and the component exists
        if (bloodEffect != null && Time.time - lastBloodEffectTime >= bloodEffectInterval) { 
            bloodEffect.ShowBloodEffect();
        }
    }

    // Helper method to apply damage to the player
    private void ApplyDamage()
    {
        if (player != null)
        {
            // Get the PlayerHealth component from the player and apply damage to the player's health
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player took 5 damage from Attack!");
            }
            else
            {
                Debug.LogWarning("No PlayerHealth script found on the player!");
            }
        }
        else
        {
            Debug.LogError("Player not found in scene!");
        }
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Vector3.Distance.html
     * https://docs.unity3d.com/ScriptReference/Time-time.html
     * https://docs.unity3d.com/ScriptReference/Canvas.html
     * https://docs.unity3d.com/ScriptReference/UI.Image.html
     * https://docs.unity3d.com/ScriptReference/Animation.html
*/