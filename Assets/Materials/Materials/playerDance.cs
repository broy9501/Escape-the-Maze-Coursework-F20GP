using UnityEngine;
using System.Collections; // Required for Coroutine

public class PlayerDance : MonoBehaviour
{
    private Animator animator;
    public AudioSource danceMusic; // Optional: Drag an audio source in the Unity Inspector

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press "1" key
        {
            StartCoroutine(DanceRoutine()); // Start dancing and stop after 5s
        }
    }

    IEnumerator DanceRoutine()
    {
        animator.SetTrigger("flair"); // Start dance animation
        
        if (danceMusic != null)
        {
            danceMusic.Play(); // Start music
        }

        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        animator.SetTrigger("idle"); // Return to idle (Make sure "Idle" is a trigger in Animator)
        
        if (danceMusic != null)
        {
            danceMusic.Stop(); // Stop music
        }
    }
}
