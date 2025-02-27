using UnityEngine;
using UnityEngine.AI;

// To be attached to a GameObject
public class EnemyAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding.

    // Called when the script instance is being loaded, before Start()
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // Assigns the NavMeshAgent component

        // Checks if the NavMeshAgent component was found
        if (agent == null)
        {
            Debug.LogError("No NavMesh agent found!");
        }

        // Attempts to get a Rigidbody component from the NPC for physics interactions.
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>(); // Ensures the GameObject has a Rigidbody for physics
        }

        rb.mass = 1000f; // Sets the Rigidbody's mass to 1000 units, making the object heavy and less likely to be pushed around easily
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
*/