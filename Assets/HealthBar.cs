using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to a Slider UI component

    // Sets the maximum health value and initializes current health to maximum
    public void SetMaxHealth(int health) {
        // Defines the maximum value the slider can reach and sets current slider value to maximum health
        slider.maxValue = health;
        slider.value = health;
    }

    // Updates the current health value on the slider
    public void SetHealth(int health) {
        slider.value = health; // Sets slider's current value to the provided health
    }
}

/*
     * References:
     * https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/script-Slider.html
     * https://www.youtube.com/watch?v=BLfNP4Sc_iA&list=LL&index=1&t=831s
*/
