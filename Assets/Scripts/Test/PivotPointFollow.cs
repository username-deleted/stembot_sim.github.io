using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class tells the pivot point for the camera to follow the SIMbot and takes the SIMbot's rotation to position the camera rotation.
public class PivotPointFollow : MonoBehaviour
{

    public GameObject SIMbot;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float nani;
        //gameObject refers to the object the script is on. In this case, the pivot point of the camera on the SIMbot.
        gameObject.transform.position = new Vector3(SIMbot.transform.position.x, SIMbot.transform.position.y, SIMbot.transform.position.z);
        //Euler angles must be used when trying to set the rotation of one object to the rotation of another. Setting specific quaternion values to another quaternion value can lead to odd behaviors.
        Debug.Log("Angle 1: " + SIMbot.transform.rotation.eulerAngles.y);
        nani = SIMbot.transform.rotation.eulerAngles.y;
        Debug.Log("Angle 2: " + nani);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(gameObject.transform.rotation.eulerAngles.x, SIMbot.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z));
    }
}
