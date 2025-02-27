using UnityEngine;
using static System.Net.WebRequestMethods;

// Class for the player's movement
public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 4.0f; // Walking speed of player
    public float sprintSpeed = 5.5f; // Sprinting speed of player
    private float currentSpeed; // Track current movement speed

    public Rigidbody rb; // Reference to player's Rigidbody
    public bool isPlayerOnFloor = true; // Checks if player is on the ground
    public Transform cameraTransform; // Reference to camera's transform for directional movement
    private Animator animator; // Reference to Animator component for animation control

    public AudioSource footstepAudio; // Audio source component for player footstep sounds
    
    // Sound clip for walking and sprinting
    public AudioClip walkSound;
    public AudioClip runSound; 

    // Time interval between walking and sprinting footsteps
    public float walkStepInterval = 0.4f; 
    public float runStepInterval = 0.2f; 
    private float stepTimer; // Timer to control footstep sound playback timing

    public AudioSource jumpAudio; // Jump sound audio source
    public AudioClip jumpSound; // Sound clip when jumping

    private bool canJump = false; // Control whether the player can jump to prevent multiple jumps

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Assign the Rigidbody component attached to this GameObject
        animator = GetComponent<Animator>(); // Assigns the Animator component attached to this GameObject

        // Assigns the AudioSource component for footsteps and sets inital speed to walking speed
        footstepAudio = GetComponent<AudioSource>();
        currentSpeed = walkSpeed; 

        // Ensure the AudioSource is NOT looping
        footstepAudio.loop = false;

        // Assign the jump AudioSource and configure it to prevent looping and playing from the start
        jumpAudio = gameObject.AddComponent<AudioSource>(); 
        jumpAudio.clip = jumpSound;
        jumpAudio.loop = false;
        jumpAudio.playOnAwake = false;
    }

    void Update()
    {
        // Get horizontal (A/D or Left/Right) and vertical (W/S or Up/Down) input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get forward and right vectors from the camera for directional movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Removes vertical components to keep movement on the horizontal plane and normalize it for consistent speed in all directions
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combines input into a movement vector relative to camera orientation
        Vector3 movement = forward * moveVertical + right * moveHorizontal;

        // Sprinting logic, increases speed when Left Shift is held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Moves the player using Translate in world space, scaled by speed and frame time and update animation based on movement speed
        transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
        animator.SetFloat("Speed", movement.magnitude);

        // Handle Footstep Sounds with Correct Timing
        if (movement.magnitude > 0.1f && isPlayerOnFloor)
        {
            stepTimer += Time.deltaTime;

            float stepInterval = (currentSpeed == sprintSpeed) ? runStepInterval : walkStepInterval;

            if (stepTimer >= stepInterval)
            {
                footstepAudio.Stop();
                footstepAudio.clip = (currentSpeed == sprintSpeed) ? runSound : walkSound;
                footstepAudio.Play();

                stepTimer = 0f; // Reset the timer
            }
        }
        else
        {
            stepTimer = 0f; // Reset timer when not moving
        }

        // Handles jumping when the jump space button is pressed
        if (Input.GetButtonDown("Jump") && isPlayerOnFloor && canJump)
        {
            // Applies an upward force for jumping and checks if player is on the floor and preveent mid-air jumps
            rb.AddForce(new Vector3(0, 4.5f, 0), ForceMode.Impulse);
            isPlayerOnFloor = false;
            canJump = false;
            animator.SetBool("isJumping", true); // Trigger jump animation

            if (!jumpAudio.isPlaying)
            {
                jumpAudio.Play(); // Play jump sound
            }
        }
    }

    // Checks if the collided object is tagged "Ground"
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isPlayerOnFloor = true;
            animator.SetBool("isJumping", false);

            // Allows jumping again after landing to prevent immediate re-jumping
            Invoke(nameof(EnableJump), 0.1f);
        }
    }

    // Ensures the player correctly detects ground contact on surfaces
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isPlayerOnFloor = true;
        }
    }

    // Enables jumping after a short delay following landing
    private void EnableJump()
    {
        canJump = true;
    }
}


/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Rigidbody.html
     * https://www.youtube.com/watch?v=1uW-GbHrtQc
     * https://www.youtube.com/watch?v=_QajrabyTJc
     * https://www.youtube.com/watch?v=cKPdSKBM4rs
     * https://www.youtube.com/watch?v=6OT43pvUyfY
*/