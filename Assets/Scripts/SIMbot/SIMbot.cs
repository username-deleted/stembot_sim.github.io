using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SIMbot : MonoBehaviour
{
    private const string filename = "/data.txt"; //where the data is saved

    public GameObject[] attachments; //a list of attachments
    public int attachmentNumber = 1; //the current attachment
    private int MAX_ATTACHMENT_INDEX; //the maximum allowed attachments, set on awake
    public GameObject attachmentSlot; //the location of the attachment

    public bool pythonBot = false; //whether the bot is using python or not
    public bool tankControls = true; //whether the bot is using tank controls or not
    public bool LEDOn = false; //whether the led is on or not
    private Light LED; //the led

    public SIMbotData SBData; //the data on the bot to be saved

    private PlayerInput playerInputComponent;

    private SaveManager saveManager; //the save manager
    private LevelManager levelManager; //the level manager

    public SimpleCarController carControllerScript;

    public Camera mainBotCamera; //the camera that follows the bot
    public OrbitCamBehaviour orbitCameraScript; //the script that controls the camera

    private void Awake()
    {
        MAX_ATTACHMENT_INDEX = attachments.Length - 1;
    }
    private void Start()
    {
        LED = GameObject.FindGameObjectWithTag("LEDLight").GetComponent<Light>();
        attachmentSlot = GameObject.FindGameObjectWithTag("AttachmentSlot");
        saveManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SaveManager>();
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();
        playerInputComponent = gameObject.GetComponent<PlayerInput>();

        //initialize bot data
        InitSBData();
        //spawn the correct attachment
        spawnAttachment();
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
    }

    //initializes the data for the SIMbot
    private void InitSBData()
    {
        //if there exists saved data for our bot, load it, otherwise make a default instance
        string path = SaveSystem.SAVE_FOLDER + filename;
        if (File.Exists(path))
        {
            Debug.Log("Got Data!");
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
    }

    //enable the main camera orbit script
    public void EnableCameraOrbit()
    {
        orbitCameraScript.enabled = true;
    }

    //disable the main camera orbit script
    public void DisableCameraOrbit()
    {
        orbitCameraScript.enabled = false;
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

    //set the current simbot variables to the data, this does not pull from the file
    private void LoadSIMbotOptions()
    {
        attachmentNumber = SBData.attachmentNumber;
        pythonBot = SBData.pythonBot;
        tankControls = SBData.tankControls;
        LEDOn = SBData.LEDOn;
    }

    //stores the current simbot variables to the data, this does not push to the file
    public void SaveSIMbotOptions()
    {
        SBData.attachmentNumber = attachmentNumber;
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
