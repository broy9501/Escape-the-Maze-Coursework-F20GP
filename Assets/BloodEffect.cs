using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodEffect : MonoBehaviour
{
    private Image bloodImage; // To store reference to the UI Image component 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Image component and set initial colour of bloodImage to red
        bloodImage = GetComponent<Image>();
        bloodImage.color = new Color(1, 0, 0, 0);
    }

    // Method to trigger the blood effect to make it visible temporarily
    public void ShowBloodEffect()
    {
        // Sets the bloodImage color to red (RGB: 1, 0, 0) with 50% opacity for a semi-transparent effect
        bloodImage.color = new Color(1, 0, 0, 0.5f);
        // Starts the HideAfterDelay coroutine to fade the effect after a delay
        StartCoroutine(HideAfterDelay());
    }

    // Coroutine to handle the timing of hiding the blood effect
    private IEnumerator HideAfterDelay()
    {
        // Pauses execution for 0.3 seconds, keeping the blood effect visible
        yield return new WaitForSeconds(0.3f);
        bloodImage.color = new Color(1, 0, 0, 0); // Fully transparent
    }

}

/*
     * References:
     * https://docs.unity3d.com/ScriptReference/Color.html
     * https://docs.unity3d.com/ScriptReference/UI.Image.html
*/
