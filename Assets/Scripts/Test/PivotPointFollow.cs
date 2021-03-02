using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPointFollow : MonoBehaviour
{

    public GameObject SIMbot;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //gameObject refers to the object the script is on. In this case, the pivot point of the camera on the SIMbot.
        gameObject.transform.position = new Vector3(SIMbot.transform.position.x, SIMbot.transform.position.y, SIMbot.transform.position.z);
        gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, SIMbot.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
    }
}
