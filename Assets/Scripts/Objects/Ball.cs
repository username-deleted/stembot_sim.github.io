using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private ScoreManager scoreManager;
    public int scoreValue;


    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();
    }

    //When the SIMBot collides with the coin, increase the score.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            scoreManager.addScore(scoreValue);
            Destroy(gameObject); //destroy is very intesive, consider cleaning this up later by diabling the object instead
        }
    }
}
