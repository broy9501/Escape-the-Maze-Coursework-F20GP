using UnityEngine;
using UnityEngine.AI;

// Behaviour petrol state in a Animator state machine for the enemy NPC
public class Patrol : StateMachineBehaviour
{
    private GameObject NPC; // Reference to NPC
    private GameObject player; // Reference to player
    private GameObject[] waypoints; // Array of waypoints references
    private int currentWP; // Index of current waypoint in waypoints
    private NavMeshAgent agent; // Reference to the NavMeshAgent

    public float chaseDistance = 3.0f; // Distance threshold for triggering chase state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC = animator.gameObject; // Assign the NPC as the GameObject attached to the Animator
        player = GameObject.FindGameObjectWithTag("Player"); // Find player in the scene by its tag
        waypoints = GameObject.FindGameObjectsWithTag("waypoint"); // Find all waypoints in the scene with the tag

        // Check if waypoints were found
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints found!");
            return;
        }

        // Get the NavMeshAgent component from the NPC for navigation
        agent = NPC.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on NPC");
            return;
        }

        agent.stoppingDistance = 1.0f; // Distance at which the agent stops near the destination
        agent.autoRepath = true; // Enable automatic repathing if the path becomes invalid

        // Randomly select an initial waypoint and set it as the destination
        currentWP = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[currentWP].transform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Exit if waypoints, agent, or player are not initialized
        if (waypoints.Length == 0 || agent == null || player == null) return;
        if (agent.pathPending) return; // Skip update if agent still calculating its path

        // Calculate distance between NPC and player and if player within chaseDistance, trigger "Chase" state in the animation
        float distanceToPlayer = Vector3.Distance(NPC.transform.position, player.transform.position);
        if (distanceToPlayer <= chaseDistance)
        {
            animator.SetTrigger("Chase");
            return;
        }

        // Continue patrolling if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            GameObject nextWaypoint;
            // Select a new random waypoint, ensuring it's not too close to the current position
            do
            {
                nextWaypoint = waypoints[Random.Range(0, waypoints.Length)];
            } while (Vector3.Distance(nextWaypoint.transform.position, NPC.transform.position) < 2.0f);
            // Add a small random offset to the waypoint position for variety
            Vector3 randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            agent.SetDestination(nextWaypoint.transform.position + randomOffset);
        }
    }
}

/*
     * References:
     * https://docs.unity3d.com/Manual/StateMachineBehaviours.html
     * https://www.youtube.com/watch?v=NEvdyefORBo
     * https://www.youtube.com/watch?v=tdYsq96kCYI&t=215s
     * https://www.youtube.com/watch?v=5qDadIloxvU
*/