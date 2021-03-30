using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>Class <c>SIMbot</c> holds all information relevent to the SIMbot. How to save it, what attachment it has, whether the LED is on or off, what color it is, whether it is a Python bot or not, what camera mode it's using, and how fast the SIMbot is moving.</summary>
public class SIMbot : MonoBehaviour
{
    /// <summary>Field <c>filename</c> is where the data is saved.</summary>
    private const string filename = "/data.txt";

    /// <summary>Property <c>attachments</c> is a list of attachments the SIMbot can have.</summary>
    public GameObject[] attachments;
    /// <summary>Property <c>attchmentNumber</c> is the current attachment on the SIMbot.</summary>
    public int attachmentNumber = 1;
    /// <summary>Field <c>MAX_ATTACHMENT_INDEX</c> is the maximum allowed attachments, set on awake.</summary>
    private int MAX_ATTACHMENT_INDEX;
    /// <summary>Property <c>attachmentSlot</c> is the location of the attachment.</summary>
    public GameObject attachmentSlot; //the location of the attachment


    //Optimization Note: A new object could be made that has both the common name and hex name. Then an array of these objects could be made. This might make the code cleaner and could make it easier to add new colors.
    /// <summary>Property <c>currentColorIndex</c> is the current color index of the SIMbot chassis and is correlated with <c>colorCommonName</c> and <c>colorHexName</c>.</summary>
    public int currentColorIndex = 0;
    /// <summary>Field <c>colorCommonName</c> is an array of colors by their common name. If adding a new color, make sure to add the hex name into the same index as the common name.</summary>
    private String[] colorCommonName = { "Default Blue", "Blue", "Light Blue", "Purple", "Magenta", "Pink", "Light Pink", "Red", "Dark Red", "Brown", "Orange", "Gold", "Yellow", "Lime Green", "Forest Green", "Spring Green", "Cyan", "White", "Light Gray", "Dark Gray", "Black"};
    /// <summary>Field <c>colorHexName</c> is an array of colors by their hex name. If adding a new color, make sure to add the common name into the same index as the hex name.</summary>
    private String[] colorHexName = { "#414EBE", "#051EDB", "#006FFF", "#5906DB", "#AD00FF", "#FF1FDA", "#FF7AF2", "#FF000A", "#800300", "#522E23", "#FF3E00", "#FF9200", "#FFED00", "#14FF00", "#064D00", "#00FF6F", "#00F8FF", "#FFFFFF", "#B5B5B5", "#767676", "#161616" };
    /// <summary>Field <c>MAX_COLOR_INDEX</c> is the maximum allowed colors, set on awake.</summary>
    private int MAX_COLOR_INDEX;

    /// <summary>Property <c>pythonBot</c> is whether the bot is using python or not.</summary>
    public bool pythonBot = false;
    /// <summary>Property <c>tankControls</c> is whether the bot is using tank controls or not.</summary>
    public bool tankControls = true;
    /// <summary>Property <c>LEDOn</c> is whether the led is on or not.</summary>
    public bool LEDOn = false;
    /// <summary>Field <c>LED</c> is the light for the led.</summary>
    private Light LED;

    /// <summary>Property <c>SBData</c> is the data on the bot to be saved.</summary>
    public SIMbotData SBData;
    /// <summary>Property <c>pivotPointFollowScript</c> is the script that controls how the pivot point follows the SIMbot.</summary>
    public PivotPointFollow pivotPointFollowScript;

    /// <summary>Field <c>playerInputComponent</c> is the input from the user on the SIMbot.</summary>
    private PlayerInput playerInputComponent;

    /// <summary>Field <c>saveManager</c> is the save manager.</summary>
    private SaveManager saveManager;
    /// <summary>Field <c>levelManager</c> is the level manager.</summary>
    private LevelManager levelManager;

    /// <summary>Field <c>simpleCarControllerScript</c> is the script on the SIMbot that controls how the SIMbot is moving.</summary>
    private SimpleCarController simpleCarControllerScript;

    /// <summary>Property <c>mainBotCamera</c> is the Camera that follows the bot.</summary>
    public Camera mainBotCamera;
    /// <summary>Property <c>orbitCameraScript</c> is the script that controls how the camera orbits around the SIMbot while the SIMbot is not moving.</summary>
    public OrbitCamBehaviour orbitCameraScript;

    /// <summary>Property <c>Speed</c> is the current speed of the SIMbot.</summary>
    public float speed;
    /// <summary>Property <c>updateDelay</c> is how long the system should wait before updating the speed of the SIMbot.</summary>
    public float updateDelay;

    private void Awake()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
        MAX_COLOR_INDEX = colorHexName.Length - 1;
        simpleCarControllerScript = GetComponent<SimpleCarController>();
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
        SpawnAttachment();
        //set the SIMBot color
        SetColor();
        //correctly set the led on or off
        UpdateLED();

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
        simpleCarControllerScript.tankControls = tankControls;

        //track the speed
        OnEnabled();
    }

    /// <summary>Method <c>InitSBData</c> initializes the data for the SIMbot.</summary>
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

    /// <summary>Method <c>updateLED</c> updates the light of the LED based on if LEDOn is true or false.</summary>
    private void UpdateLED() {
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

    /// <summary>Method <c>NextAttachment</c> sets the SIMbot's attachment to the next attachment in the <c>attachments</c> array.</summary>
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
        ClearAttachments();
        SpawnAttachment();
    }

    /// <summary>Method <c>PreviousAttachment</c> sets the SIMbot's attachment to the previous attachment in the <c>attachments</c> array.</summary>
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
        ClearAttachments();
        SpawnAttachment();
    }

    /// <summary>Method <c>spawnAttachment</c> initializes the attachment on the SIMbot.</summary>
    private void SpawnAttachment()
    {
        if (attachments[attachmentNumber] != null)
        {
            GameObject currentAttachment = Instantiate(attachments[attachmentNumber], attachmentSlot.transform.position, attachmentSlot.transform.rotation);
            currentAttachment.transform.parent = attachmentSlot.transform;
        }
    }

    /// <summary>Method <c>ClearAttachments</c> clears the current attachments on the SIMbot.</summary>
    private void ClearAttachments()
    {
        foreach (Transform child in attachmentSlot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>Method <c>NextColor</c> sets the SIMbot's color to the next color in the <c>colorHexName</c> array by increasing the index.</summary>
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

    /// <summary>Method <c>PreviousColor</c> sets the SIMbot's color to the previous color in the <c>colorHexName</c> array by decreasing the index.</summary>
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

    /// <summary>Method <c>GetColor</c> gets the SIMbot's current color by common name.</summary>
    /// <returns>A string representing the SIMbot's current color by common name.</returns>
    public String GetColor() {
        return colorCommonName[currentColorIndex];
    }

    public void SetColor() {
        Renderer chassisColor = GameObject.FindGameObjectWithTag("SIMBotCollider").GetComponent<Renderer>();
        Color newColor;
        //converts the hexColor into a color that we can then set at the chassis color. 
        if (ColorUtility.TryParseHtmlString(colorHexName[currentColorIndex], out newColor)) {
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
        simpleCarControllerScript.tankControls = tankControls;

        //Save it in SBData to persist between scenes.
        SBData.tankControls = tankControls;
    }

    public void ToggleLED()
    {
        LEDOn = !LEDOn;
        UpdateLED();
        //Save it in SBData to persist between scenes.
        SBData.LEDOn = LEDOn;
    }

    //toggle the main camera orbit script on and off
    public void ToggleCameraOrbit()
    {
        orbitCameraScript.enabled = !orbitCameraScript.enabled;
        pivotPointFollowScript.enabled = !pivotPointFollowScript.enabled;
    }

    //enable the main camera orbit script
    public void EnableCameraOrbit()
    {
        orbitCameraScript.enabled = true;
        pivotPointFollowScript.enabled = true;
    }

    //disable the main camera orbit script
    public void DisableCameraOrbit()
    {
        orbitCameraScript.enabled = false;
        pivotPointFollowScript.enabled = false;
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

        YieldInstruction timedWait = new WaitForSeconds(updateDelay);
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
                speed = deltaPosition / deltaTime;
            }

            if (GameObject.FindGameObjectWithTag("SpeedText") != null)
            {
                Text scoreDisplay = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<Text>();
                scoreDisplay.text = Math.Round(speed).ToString();
            }

            lastPosition = transform.position;
            lastTimestamp = Time.time;
        }
    }

    public double getSpeed()
    {
        return speed;
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
