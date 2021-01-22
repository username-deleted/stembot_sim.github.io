using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{

    public GameManagerBehaviour sceneManager;
    public GameObject effect;
    public int spinSpeed = 5;

    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
    }

    /*OnCollisionEnter(Collision)
    Desc: This method is called whenever a gameObject collides with this gameObject. It gets the
        information of the gameObject it collided with. It increments the score, and then 
        destroys itself.
    Parameters: None
    Return: None*/
    private void OnCollisionEnter(Collision otherObject) {
        if (otherObject.gameObject.tag == "Player") {
            if(!collected) {
                collected = true;
                sceneManager.AddToScore(1);
                Destroy(this.gameObject);

                //Add these lines below if you want to add an effect when the coin is picked up.
                //GameObject boom = Instantiate (effect, transform.position, transform.rotation);
                //Destroy(boom.gameObject, 10);
                
            }
        }
    }
}
