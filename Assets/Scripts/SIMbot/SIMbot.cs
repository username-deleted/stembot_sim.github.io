using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>Class <c>SIMbot</c> holds all information relevent to the SIMbot. How to save it, what attachment it has, whether the LED is on or off, what color it is, whether it is a Python bot or not, what camera mode it's using, and how fast the SIMbot is moving.</summary>
public class SIMbot : MonoBehaviour
{
    private const string filename = "/data.txt"; //where the data is saved

    public GameObject[] attachments; //a list of attachments
    public int attachmentNumber = 1; //the current attachment
    private int MAX_ATTACHMENT_INDEX; //the maximum allowed attachments, set on awake
    public GameObject attachmentSlot; //the location of the attachment

    public int currentColorIndex = 0;//the current color index
    private String[] colorCommonName = { "Default Blue", "Blue", "Light Blue", "Purple", "Magenta", "Pink", "Light Pink", "Red", "Dark Red", "Brown", "Orange", "Gold", "Yellow", "Lime Green", "Forest Green", "Spring Green", "Cyan", "White", "Light Gray", "Dark Gray", "Black"};
    private String[] colorsHexName = { "#414EBE", "#051EDB", "#006FFF", "#5906DB", "#AD00FF", "#FF1FDA", "#FF7AF2", "#FF000A", "#800300", "#522E23", "#FF3E00", "#FF9200", "#FFED00", "#14FF00", "#064D00", "#00FF6F", "#00F8FF", "#FFFFFF", "#B5B5B5", "#767676", "#161616" };
    private int MAX_COLOR_INDEX;

    public bool pythonBot = false; //whether the bot is using python or not
    public bool tankControls = true; //whether the bot is using tank controls or not
    public bool LEDOn = false; //whether the led is on or not
    private Light LED; //the led

    public SIMbotData SBData; //the data on the bot to be saved
    public PivotPointFollow cameraPivotPointScript;

    private PlayerInput playerInputComponent;

    private SaveManager saveManager; //the save manager
    private LevelManager levelManager; //the level manager

    private SimpleCarController carControllerScript;

    public Camera mainBotCamera; //the camera that follows the bot
    public OrbitCamBehaviour orbitCameraScript; //the script that controls the camera

    public float Speed;//speed of the SIMbot.
    public float UpdateDelay;//How long to wait before updating the speed.

    private void Awake()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
        MAX_COLOR_INDEX = colorsHexName.Length - 1;
        carControllerScript = GetComponent<SimpleCarController>();
        playerInputComponent = GetComponent<PlayerInput>();

    }
    private void Start()
    {
        LED = GameObject.FindGameObjectWithTag("LEDLight").GetComponent<Light>();
        attachmentSlot = GameObject.FindGameObjectWithTag("AttachmentSlot");
        saveManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SaveManager>();
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();
        //Can be cleaned up, this is currently necessary to keep the settings on the MainMenu SIMbot functioning.
        if (mainBotCamera == null) {
            mainBotCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        if (orbitCameraScript == null) {
            orbitCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OrbitCamBehaviour>();
        }

        
        //initialize bot data
        InitSBData();
        //spawn the correct attachment
        spawnAttachment();
        //set the SIMBot color
        SetColor();
        //correctly set the led on or off
        updateLED();

        //if we're in the main menu scene, disable the bot camera, orbiting script, and player input
        if (levelManager.InMainMenuScene())
        {
            DisableCamera();
            DisableCameraOrbit();
            DisablePlayerInputComponent();
        }
        else
        //if we aren't in the main menu scene, enable the bot camera, orbiting script, and player input
        {
            EnableCamera();
            EnableCameraOrbit();
            EnablePlayerInputComponent();
        }

        //set the controller controls
        carControllerScript.tankControls = tankControls;

        //track the speed
        OnEnabled();
    }

    //initializes the data for the SIMbot
    private void InitSBData()
    {
        //if there exists saved data for our bot, load it, otherwise make a default instance
        string path = SaveSystem.SAVE_FOLDER + filename;
        if (File.Exists(path))
        {
            //Debug.Log("Got Data!");
            SIMbotData data = LoadSIMbotDataFromFile();

            //Debug.Log(data.attachmentNumber);
            //Debug.Log(data.pythonBot);
            //Debug.Log(data.tankControls);
            //Debug.Log(data.LEDOn);

            //update the bot's copy of the data (if we want to save it later)
            UpdateSBData(data);
        }
        else
        {
            //there was no data, use defaults
            SBData = new SIMbotData();
        }
        //update the bot's variables (for use within the game). this relies on SBData, so always run UpdateSBData beforehand
        //or instantiate a default object of SBData, in other words, make sure SBData is not null before calling LoadSIMBotOptions
        LoadSIMbotOptions();
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

    public void NextColor()
    {
        if (currentColorIndex == MAX_COLOR_INDEX)
        {
            currentColorIndex = 0;
        }
        else
        {
            currentColorIndex++;
        }
        //Save it in SBData to persist between scenes.
        SBData.currentColor = currentColorIndex;
        SetColor();
    }

    public void PreviousColor()
    {
        if (currentColorIndex == 0)
        {
            currentColorIndex = MAX_COLOR_INDEX;
        }
        else
        {
            currentColorIndex--;
        }
        //Save it in SBData to persist between scenes.
        SBData.currentColor = currentColorIndex;
        SetColor();
    }

    public String GetColor() {
        return colorCommonName[currentColorIndex];
    }

    public void SetColor() {
        Renderer chassisColor = GameObject.FindGameObjectWithTag("SIMBotCollider").GetComponent<Renderer>();
        Color newColor;
        //converts the hexColor into a color that we can then set at the chassis color. 
        if (ColorUtility.TryParseHtmlString(colorsHexName[currentColorIndex], out newColor)) {
            chassisColor.material.color = newColor;
        }
    }

    //set the python bot to true or false
    public void SetPythonBot(bool value)
    {
        pythonBot = value;

        //Save it in SBData to persist between scenes.
        SBData.pythonBot = pythonBot;
    }

    //set tank controls to true or false
    public void SetTankControls(bool value)
    {
        tankControls = value;

        //update the car controller's controls
        carControllerScript.tankControls = tankControls;

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

    //toggle the main camera orbit script on and off
    public void ToggleCameraOrbit()
    {
        orbitCameraScript.enabled = !orbitCameraScript.enabled;
        cameraPivotPointScript.enabled = !cameraPivotPointScript.enabled;
    }

    //enable the main camera orbit script
    public void EnableCameraOrbit()
    {
        orbitCameraScript.enabled = true;
        cameraPivotPointScript.enabled = true;
    }

    //disable the main camera orbit script
    public void DisableCameraOrbit()
    {
        orbitCameraScript.enabled = false;
        cameraPivotPointScript.enabled = false;
    }

    //enable the camera
    private void EnableCamera()
    {
        mainBotCamera.enabled = true;
    }

    //disable the camera
    private void DisableCamera()
    {
        mainBotCamera.enabled = false;
    }

    //enable player input component
    private void EnablePlayerInputComponent()
    {
        playerInputComponent.enabled = true;
    }

    //disable player input component, renders player unable to do any input through the bot
    private void DisablePlayerInputComponent()
    {
        playerInputComponent.enabled = false;
    }


    void OnEnabled()
    {
        StartCoroutine(SpeedReckoner());
    }

    private IEnumerator SpeedReckoner()
    {

        YieldInstruction timedWait = new WaitForSeconds(UpdateDelay);
        Vector3 lastPosition = transform.position;
        float lastTimestamp = Time.time;

        while (enabled)
        {
            yield return timedWait;

            var deltaPosition = (transform.position - lastPosition).magnitude;
            var deltaTime = Time.time - lastTimestamp;

            if (Mathf.Approximately(deltaPosition, 0f)) // Clean up "near-zero" displacement
                deltaPosition = 0f;

            if (deltaPosition / deltaTime != 0)
            {
                Speed = deltaPosition / deltaTime;
            }

            if (GameObject.FindGameObjectWithTag("SpeedText") != null)
            {
                Text scoreDisplay = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
                scoreDisplay.text = Math.Round(Speed).ToString();
            }

            lastPosition = transform.position;
            lastTimestamp = Time.time;
        }
    }

    public double getSpeed()
    {
        return Speed;
    }

    //set the current simbot variables to the data, this does not pull from the file
    private void LoadSIMbotOptions()
    {
        attachmentNumber = SBData.attachmentNumber;
        currentColorIndex = SBData.currentColor;
        pythonBot = SBData.pythonBot;
        tankControls = SBData.tankControls;
        LEDOn = SBData.LEDOn;
    }

    //stores the current simbot variables to the data, this does not push to the file
    public void SaveSIMbotOptions()
    {
        SBData.attachmentNumber = attachmentNumber;
        SBData.currentColor = currentColorIndex;
        SBData.pythonBot = pythonBot;
        SBData.tankControls = tankControls;
        SBData.LEDOn = LEDOn;
    }

    //save the sbdata to a file
    public void SaveSIMbotDataToFile()
    {
        saveManager.SaveSBData();
    }

    //load the sbdata from a file
    public SIMbotData LoadSIMbotDataFromFile()
    {
        return JsonUtility.FromJson<SIMbotData>(saveManager.LoadSBData());
    }

    //changes the data saved to the newly given data
    public void UpdateSBData(SIMbotData data)
    {
        SBData = new SIMbotData(data.attachmentNumber, data.currentColor, data.pythonBot, data.tankControls, data.LEDOn);
    }

    public void SayHi()
    {
        Debug.Log("Hi!");
    }

    public class SIMbotData
    {
        public int attachmentNumber = 0;
        public int currentColor = 0;
        public bool pythonBot = false;
        public bool tankControls = true;
        public bool LEDOn = true;
        public SIMbotData()
        {
            attachmentNumber = 0;
            currentColor = 0;
            pythonBot = false;
            tankControls = true;
            LEDOn = true;
        }

        public SIMbotData(int attachmentNumber, int currentColor, bool pythonBot, bool tankControls, bool LEDOn)
        {
            this.attachmentNumber = attachmentNumber;
            this.currentColor = currentColor;
            this.pythonBot = pythonBot;
            this.tankControls = tankControls;
            this.LEDOn = LEDOn;
        }
    }
}
