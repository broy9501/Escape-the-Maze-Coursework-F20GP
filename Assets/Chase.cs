using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Behaviour chase state in a Animator state machine for the enemy NPC
public class Chase : StateMachineBehaviour
{

    private GameObject NPC; // Reference to NPC 
    private GameObject player; // Reference to player
    private NavMeshAgent agent; //NavMeshAgent component for pathfinding and movement

    public float loseSightDistance = 5.0f; // Define the distance at which the NPC loses sight of the player
    public Vector3 lastDestination; // Stores the last known position of the player for navigation
    private float updateThreshold = 2.0f; // Threshold distance to decide when to update the destination

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC = animator.gameObject; // Assign the NPC GameObject from the animator
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player in the scene using a tag
        agent = NPC.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component from the NPC

        // Check for missing components and log an error if they aren't found
        if (agent == null || player == null)
        {
            Debug.LogError("Chase: Missing NavMeshAgent or Player!");
            return;
        }

        agent.speed = 5f; // Agent's movement speed
        agent.angularSpeed = 900f; // Set rotation speed for quick turning
        agent.acceleration = 12f; // Increase acceleration for faster speed build-up
        agent.autoBraking = false; // Disable braking for continous movement
        agent.autoRepath = false; // Disable automatic repathing to update manually

        // Distance at which the agent stops near the target and disable root motion
        agent.stoppingDistance = 0.5f;
        animator.applyRootMotion = false;

        // Initialize last destination
        lastDestination = player.transform.position;
        agent.SetDestination(lastDestination);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (agent == null || player == null) { return; }

        // Calculate the distance between the NPC and the player
        float distanceToPlayer = Vector3.Distance(NPC.transform.position, player.transform.position);

        // If the NPC is close enough to attack
        if (distanceToPlayer <= 2.0f) {
            // Define the raycast origin slightly above the NPC's position and calculate the direction toward the player
            Vector3 origin = NPC.transform.position + Vector3.up;
            Vector3 direction = (player.transform.position - origin).normalized;

            // Perform a raycast to check if the player is in line of sight
            if (Physics.Raycast(origin, direction, out RaycastHit hit, distanceToPlayer)) {
                // If the ray hits the player, trigger an attack
                if (hit.collider.CompareTag("Player")) {
                    animator.SetTrigger("Attack");
                    return;
                }
            }
        }

        // If the player is too far away, switch to patrol mode
        if (distanceToPlayer > loseSightDistance) {
            animator.SetTrigger("Patrol");
            return;
        }

        // Update the destination if the player has moved beyond the threshold
        if (Vector3.Distance(lastDestination, player.transform.position) > updateThreshold) { 
            lastDestination = player.transform.position;
            agent.SetDestination(lastDestination);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger("Attack"); // Reset the attack trigger to avoid unintended transitions
        //animator.SetTrigger("Patrol");
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html
     * https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
*/