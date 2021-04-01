using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int MAX_HEALTH = 100;
    private int health;

    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        Time.timeScale = 1;
    }

    public void AddHealth(int health_value)
    {
        health = health + health_value;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        healthBar.SetHealth(health);
    }

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

    public int getHealth()
    {
        return health;
    }
}