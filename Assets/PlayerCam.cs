using Unity.VisualScripting;
using UnityEngine;

// Class for controlling the player's camera movement from mouse input
public class PlayerCam : MonoBehaviour
{
    public Transform player; // Reference to player's transform for rotating player
    public float mouseSens = 2f; // Mouse Sensitivity to control how fast the camera moves
    float cameraVerticalRotation = 0f; // Camera's cuurent vertical rotation angle

    // Update is called once per frame
    void Update()
    {
        // Horizontal and vertical mouse movement and multiplied by the sensitivity
        float inputX = Input.GetAxis("Mouse X") * mouseSens;
        float inputY = Input.GetAxis("Mouse Y") * mouseSens;

        // Update and clamp vertical rotation and apply vertical rotation to camera 
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        player.Rotate(Vector3.up * inputX); // Rotate player horizontally based on mouse's X movement
    }
}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
     * https://www.youtube.com/watch?v=_QajrabyTJc
     * https://www.youtube.com/watch?v=NjjRkms8ODk
*/