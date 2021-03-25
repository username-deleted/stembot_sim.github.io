using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class tells the pivot point for the camera to follow the SIMbot and takes the SIMbot's y rotation to rotation the camera.
public class PivotPointFollow : MonoBehaviour
{
    private GameObject SIMbot;
    private OrbitCamBehaviour OCBScript;
    private SIMbot simbotScript;

    private void Start()
    {
        SIMbot = GameObject.FindGameObjectWithTag("Player");
        simbotScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SIMbot>();
        OCBScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OrbitCamBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {
        //gameObject refers to the object the script is on. In this case, it's the pivot point of the camera.
        gameObject.transform.position = new Vector3(SIMbot.transform.position.x, SIMbot.transform.position.y, SIMbot.transform.position.z);

        //The enabled/disabled section is unoptimized because it runs during every update. To fix this later, fire an event when the SIMbot is moving/not moving.
        //If moving, use the OCBScript.
        if (simbotScript.getSpeed() >= .01)
        {
            OCBScript.enabled = false;
            //Euler angles must be used when trying to set the rotation of one object to the rotation of another. Setting specific quaternion values to another quaternion value can lead to odd behaviors if you don't know what a Quaternion is.
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.eulerAngles.x, SIMbot.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z));
        }
        else {
            OCBScript.enabled = true;
        }
    }
}
