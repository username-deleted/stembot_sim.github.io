using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    public GameManagerBehaviour sceneManager;
    public GameObject effect;
    public int value = 1;

    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        //sceneManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*OnCollisionEnter(Collision)
    Desc: This method is called whenever a gameObject collides with this gameObject. It gets the
        information of the gameObject it collided with. This is added to the ball gameObjects for
        level 2. If it detects that it collided with the goal, it will increment the score, and then 
        destroys itself.
    Parameters: None
    Return: None*/
    private void OnCollisionEnter(Collision otherObject) {
        if (otherObject.gameObject.tag == "Goal") {
            if(!collected) {
                collected = true;
                sceneManager.AddToScore(value);
                Destroy(this.gameObject);

                //Add these lines below if you want to add an effect when the coin is picked up.
                //GameObject boom = Instantiate (effect, transform.position, transform.rotation);
                //Destroy(boom.gameObject, 10);
            }
        }
    }
}
