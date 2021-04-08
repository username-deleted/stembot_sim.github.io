using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    private int coins;
    private SC_CompletionMenu completionMenu;
    private ScoreManager scoreManager;
    private TimeManager timeManager;
    private Text completionMenuTimeText;


    void Start()
    {
        coins = GameObject.FindGameObjectsWithTag("Coin").Length;
        completionMenu = GameObject.FindGameObjectWithTag("CompletionCanvas").GetComponent<SC_CompletionMenu>();
        timeManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TimeManager>();
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();
        completionMenuTimeText = GameObject.FindGameObjectWithTag("CompletionMenuTimeText").GetComponent<Text>();

    }

    public void removeCoin(GameObject coin) {
        // We are assuming the SIMbot collided with the coin which is why a coin is being removed, therefore the player scored a point. 
        //If this method is being called for a different reason, the scoring mechanics need to be moved elsewhere.   
        
        coins--;
        //disable the coin so it no longer appears in the scene
        coin.SetActive(false);
        
        scoreManager.addScore(1);

        if (coins <= 0) {
            completionMenu.enableCompletionMenu();
            completionMenuTimeText.text = timeManager.getTime().ToString("F2");
        }
    }

    public int getCoins() {
        return coins;
    }
}
