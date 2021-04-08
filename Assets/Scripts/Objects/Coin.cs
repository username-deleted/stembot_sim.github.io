using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManager coinManager;

    // Start is called before the first frame update
    void Start()
    {
        coinManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<CoinManager>();
    }

    //When the SIMBot collides with the coin, increase the score.
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "SIMBotCollider") 
        {
            coinManager.removeCoin(gameObject);
        }
    }
}
