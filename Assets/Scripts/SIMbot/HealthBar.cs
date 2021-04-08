
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>Class <c>HealthBar</c> controls the ui slider visualizations for the health bar.</summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>Property <c>slider</c> is the slider for the health bar which can be slid left and right to represent the SIMbot's current health during gameplay.</summary>
    public Slider slider;

    /// <summary>Method <c>SetMaxHealth</c> sets the maximum value of the health bar and is used to initialize the health bar.</summary>
    /// <param><c>health</c> is the maximum health the SIMbot can have</param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>Method <c>SetHealth</c> sets the health equal to the variable passed in.</summary>
    /// <param><c>health</c> is the value that the SIMbot health will become.</param>
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}