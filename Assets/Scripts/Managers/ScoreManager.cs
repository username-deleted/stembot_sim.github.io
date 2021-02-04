using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int maxScore;
    private Text scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //The max score is based on how many coins are in the scene.
        maxScore = GameObject.FindGameObjectsWithTag("Coin").Length;
        scoreDisplay = GameObject.FindGameObjectWithTag("ScoreValue").GetComponent<Text>();
        scoreDisplay.text = currentScore.ToString();
    }

    //increment the score by the value passed in.
    public void addScore(int point)
    {
        currentScore += point;
        scoreDisplay.text = currentScore.ToString();
    }
}
