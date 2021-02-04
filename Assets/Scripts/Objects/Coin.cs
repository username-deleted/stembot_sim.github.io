﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();
    }

    //When the SIMBot collides with the coin, increase the score.
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "SIMBotCollider") 
        {
            scoreManager.addScore(1);
            Destroy(gameObject); //destroy is very intesive, consider cleaning this up later by diabling the object instead
        }
    }
}
