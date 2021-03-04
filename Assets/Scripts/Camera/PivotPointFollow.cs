using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class tells the pivot point for the camera to follow the SIMbot and takes the SIMbot's y rotation to rotation the camera.
public class PivotPointFollow : MonoBehaviour
{
    public GameObject SIMbot;
    public OrbitCamBehaviour OCBScript;

    public bool moving;
    public double time;

    private Vector3 previousPosition;
    //private bool isMoving = false; Events can set this to true or false later.

    private void Start()
    {
        Vector3 previousPosition = SIMbot.transform.position;
        moving = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject refers to the object the script is on. In this case, it's the pivot point of the camera.
        gameObject.transform.position = new Vector3(SIMbot.transform.position.x, SIMbot.transform.position.y, SIMbot.transform.position.z);

        //The enabled/disabled section is unoptimized because it runs during every update. To fix this later, fire an event when the SIMbot is moving/not moving.
        //if the SIMbot is moving any movement keys are being held, disable the OrbitCamBehavior Script and follow the SIMbot's rotation, otherwise, enable the OrbitCamBehavior.
        if (isMoving())
        {
            OCBScript.enabled = true;
        }
        else {
            OCBScript.enabled = false;
            //Euler angles must be used when trying to set the rotation of one object to the rotation of another. Setting specific quaternion values to another quaternion value can lead to odd behaviors if you don't know what a Quaternion is.
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.eulerAngles.x, SIMbot.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z));
        }
    }
    private bool isMoving() {
        //keep track of how much time has passed.
        time = time + Time.deltaTime;
        
        //If there has been significant movement over the last .5 seconds, then the SIMbot is considered moving.
        if (time > 2) {
            time = time - .1;
            if (
                (gameObject.transform.position.x - .2 < previousPosition.x && previousPosition.x < gameObject.transform.position.x + .2) &&
                (gameObject.transform.position.y - .2 < previousPosition.y && previousPosition.y < gameObject.transform.position.y + .2) &&
                (gameObject.transform.position.z - .2 < previousPosition.z && previousPosition.z < gameObject.transform.position.z + .2)){
                //reset the position
                previousPosition = SIMbot.transform.position;
                moving = true;
            }
            else {
                //reset the position
                previousPosition = SIMbot.transform.position;
                moving = false; 
            }
        }
        return moving;
    }
}
