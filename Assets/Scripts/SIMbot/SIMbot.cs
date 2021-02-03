﻿using System.IO;
using UnityEngine;

public class SIMbot : MonoBehaviour
{
    private const string filename = "/data.txt"; //where the data is saved
    public GameObject[] attachments;
    public int attachmentNumber = 1;
    public GameObject attachmentSlot;
    public bool pythonBot = false;
    public bool tankControls = true;
    public bool LEDOn = false;
    private Light LED;
    public SIMbotData SBData;
    private int MAX_ATTACHMENT_INDEX;


    private void Awake()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
    }
    private void Start()
    {
        LED = GameObject.FindGameObjectWithTag("LEDLight").GetComponent<Light>();
        attachmentSlot = GameObject.FindGameObjectWithTag("AttachmentSlot");
        //initialize bot data
        InitSBData();
        spawnAttachment();
        updateLED();
    }

    //initializes the data for the SIMbot
    private void InitSBData()
    {
        //if there exists saved data for our bot, load it, otherwise make a default instance
        string path = SaveSystem.SAVE_FOLDER + filename;
        if (File.Exists(path))
        {
            Debug.Log("Got Data!");
            SIMbotData data = GetSBDataFromFile();

            //Debug.Log(data.attachmentNumber);
            //Debug.Log(data.pythonBot);
            //Debug.Log(data.tankControls);
            //Debug.Log(data.LEDOn);

            //update the bot's copy of the data (if we want to save it later)
            UpdateSBData(data);
        }
        else
        {
            SBData = new SIMbotData(attachmentNumber, pythonBot, tankControls, LEDOn);
        }
        //update the bot's variables (for use within the game). this relies on SBData, so always run UpdateSBData beforehand
        LoadSIMbotOptions();
    }

    private SIMbotData GetSBDataFromFile()
    {
        return JsonUtility.FromJson<SIMbotData>(SaveSystem.Load());
    }

    //updates the variables to the saved data in the SBData variable
    private void LoadSIMbotOptions()
    {
        attachmentNumber = SBData.attachmentNumber;
        pythonBot = SBData.pythonBot;
        tankControls = SBData.tankControls;
        LEDOn = SBData.LEDOn;
    }

    private void updateLED() {
        //Debug.Log(LEDOn);
        if (LEDOn)
        {
            LED.enabled = true;
            //Debug.Log("LED should be on");
        }
        else
        {
            LED.enabled = false;
            //Debug.Log("LED should be off");
        }
    }

    public void NextAttachment()
    {
        if(attachmentNumber == MAX_ATTACHMENT_INDEX)
        {
            attachmentNumber = 0;
        }
        else
        {
            attachmentNumber++;
        }
        //Save it in SBData to persist between scenes.
        SBData.attachmentNumber = attachmentNumber;
        clearAttachments();
        spawnAttachment();
    }

    public void PreviousAttachment()
    {
        if(attachmentNumber == 0)
        {
            attachmentNumber = MAX_ATTACHMENT_INDEX;
        }
        else
        {
            attachmentNumber--;
        }
        //Save it in SBData to persist between scenes.
        SBData.attachmentNumber = attachmentNumber;
        clearAttachments();
        spawnAttachment();
    }

    //initialize the attachment on the SIMbot
    private void spawnAttachment()
    {
        if (attachments[attachmentNumber] != null)
        {
            GameObject currentAttachment = Instantiate(attachments[attachmentNumber], attachmentSlot.transform.position, attachmentSlot.transform.rotation);
            currentAttachment.transform.parent = attachmentSlot.transform;
        }
    }

    //clear the current attchments on the SIMbot
    private void clearAttachments()
    {
        foreach (Transform child in attachmentSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void TogglePythonBot()
    {
        pythonBot = !pythonBot;
        //Save it in SBData to persist between scenes.
        SBData.pythonBot = pythonBot;
    }

    public void ToggleTankControls()
    {
        tankControls = !tankControls;
        //Save it in SBData to persist between scenes.
        SBData.tankControls = tankControls;
    }

    public void ToggleLED()
    {
        LEDOn = !LEDOn;
        updateLED();
        //Save it in SBData to persist between scenes.
        SBData.LEDOn = LEDOn;
    }

    //changes the data saved to the newly given data
    public void UpdateSBData(SIMbotData data)
    {
        SBData = new SIMbotData(data.attachmentNumber, data.pythonBot, data.tankControls, data.LEDOn);
    }

    public class SIMbotData
    {
        public int attachmentNumber = 0;
        public bool pythonBot = false;
        public bool tankControls = true;
        public bool LEDOn = true;
        public SIMbotData()
        {
            attachmentNumber = 0;
            pythonBot = false;
            tankControls = true;
            LEDOn = true;
        }

        public SIMbotData(int attachmentNumber, bool pythonBot, bool tankControls, bool LEDOn)
        {
            this.attachmentNumber = attachmentNumber;
            this.pythonBot = pythonBot;
            this.tankControls = tankControls;
            this.LEDOn = LEDOn;
        }
    }
}
