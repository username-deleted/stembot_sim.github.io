using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class <c>PivorPointFollow</c> tells the pivor point of the camera to follow the SIMbot and takes the SIMbot's y rotation to orientate which way the camera is looking.</summary>
public class PivotPointFollow : MonoBehaviour
{
    /// <summary>Field <c>SIMbot</c> represents the SIMbot in the scene.</summary>
    private GameObject SIMbot;
    /// <summary>Field <c>OrbitCamBehavior</c> represents the orbital camera script on the main camera. This script is used to rotate the camera around the SIMbot while the SIMbot is not moving.</summary>
    private OrbitCamBehaviour OCBScript;
    /// <summary>Field <c>simbotScript</c> represents the SIMbot's script. The SIMbot's script keeps track of how fast the SIMbot is moving. This speed is used to determine which camera to use.<summary>
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
