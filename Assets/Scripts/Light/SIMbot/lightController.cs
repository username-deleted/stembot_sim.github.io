using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This controls the LED light on the SIMbot.
public class lightController : MonoBehaviour
{
    float duration = 1.0f; //how long the transition will take

    //Can create arrays of colors to loop through
    Color[] colors = {
        Color.red,
        Color.green,
        Color.blue
    };

    //Or you can just make a color you want to use
    //Color custom = new Color32(150, 150, 150);
    //There are also built in colors you can use
    Color color0 = Color.red;
    Color color1 = Color.green;
    Color color2 = Color.blue;

    Light lt;

    // Start is called before the first frame update
    void Start()
    {
        //grabs light
        lt = GetComponent<Light>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        lt.color = Color.Lerp(color0, color2, t); //transitions light from a color to another color

    }
}
