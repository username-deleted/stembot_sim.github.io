using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int maxScore;

    // Start is called before the first frame update
    void Start()
    {
        //The max score is based on how many coins are in the scene.
        maxScore = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*AddToScore(int)
    Desc: Increments the score variable by the value passed in.
    Parameters: int, point to be added to score
    Return: None*/
    public void addScore(int point)
    {
        score += point;
        Debug.Log(score);
    }
}
