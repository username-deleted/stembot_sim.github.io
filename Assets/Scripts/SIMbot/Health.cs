using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class <c>Health</c> manages the SIMbot's health.</summary>
public class Health : MonoBehaviour
{
    /// <summary>Property <c>filename</c> is where the data is saved.</summary>
    
    /// <summary>Field <c>MAX_HEALTH</c> is the maximum health the SIMbot can have.</summary>
    private int MAX_HEALTH = 100;

    /// <summary>Field <c>MAX_HEALTH</c></summary>
    private int health;

    /// <summary>Field <c>healthBar</c> is the script for the ui element of the health. It is used to update the visualizations of the health.</summary>
    private HealthBar healthBar;

    void Start()
    {
        health = MAX_HEALTH;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        Time.timeScale = 1;
    }

    /// <summary>Method <c>AddHealth</c> adds health to the current health.</summary>
    /// <param><c>health_value</c> is the value that will be added to the current health.</param>
    public void AddHealth(int health_value)
    {
        health = health + health_value;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        healthBar.SetHealth(health);
    }

    /// <summary>Method <c>RemoveHealth</c> removes health from the current health.</summary>
    /// <param><c>health_value</c> is the value that will be removed from the current health.</param>
    public void RemoveHealth(int health_value)
    {
        if (health - health_value <= 0)
        {
            health = 0;
            healthBar.SetHealth(health);
        }
        else
        {
            health = health - Mathf.Abs(health_value);
            healthBar.SetHealth(health);
        }
    }

    /// <summary>Method <c>getHealth</c> returns the current health value.</summary>
    public int getHealth()
    {
        return health;
    }
}