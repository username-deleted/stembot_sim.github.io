using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private ScoreManager scoreManager;
    public GameObject effect;
    public int spinSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
    }

    //When the SIMBot collides with the coin, increase the score.
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "SIMBotCollider") 
        {
            scoreManager.addScore(1);
            Destroy(gameObject);
        }
    }
}
