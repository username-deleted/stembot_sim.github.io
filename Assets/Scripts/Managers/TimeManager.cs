﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    private float currentTime = 0;
    private Text timeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        timeDisplay = GameObject.FindGameObjectWithTag("TimeValue").GetComponent<Text>();
        timeDisplay.text = currentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        timeDisplay.text = currentTime.ToString("F2");
    }
}
