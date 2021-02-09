using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public string menuScene;
    public string levelSelectScene;
    public string[] levelList;

    public GameObject SIMbotPrefab;
    public GameObject SIMbotPythonPrefab;

    private GameObject pauseMenu;
    private GameObject scoreStatusMenu;
    private GameObject finalStatusMenu;
    private GameObject SIMbot;
    private Text scoreText;
    private Text timerText;
    private Text finalTimeText;
    private SimpleCarController CarScript;
    private Light LED;

    private bool LEDon = false;
    private bool tankControls = true;
    //private bool scriptControls = false;
    private bool pythonControls = false;
    private bool InGameplay = false;

    private GameObject AttachmentSlot;
    private bool InCustomization = false;
    private int selectedLevel = 0;
    private int selectedAttachment = 0;

    public GameObject[] attachmentPrefabs;

    private int scoreLimit = 3;
    private int score = 0;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        //Find PauseMenu
        if(pauseMenu == null && GameObject.Find("PauseMenu") != null) {
            pauseMenu = GameObject.Find("PauseMenu");
            pauseMenu.SetActive(false);
        }

        //Find ScoreStatusMenu
        if(scoreStatusMenu == null && GameObject.Find("scoreStatusMenu") != null) {
            scoreStatusMenu = GameObject.Find("scoreStatusMenu");
            finalStatusMenu = GameObject.Find("scoreStatusMenu/finalStatusMenu");
            finalStatusMenu.SetActive(false);

            //Find textfields for score & timer
            scoreText = GameObject.Find("scoreStatusMenu/Score").GetComponent<Text>();
            timerText = GameObject.Find("scoreStatusMenu/Time").GetComponent<Text>();
        } else if (scoreStatusMenu != null) {
            scoreText.text = "Score: " + score + "/" + scoreLimit;
            timerText.text = "Timer: " + timer.ToString("F2");
        }

        //Find SIMbot & related gameObjects
        if (CarScript == null) {
            FindSIMbot();
            AddSelectedAttachment();
            CheckLED();
        }

        //When customizing at main menu, find & update custmization menu
        if (InCustomization) {
            Text attachNameText = GameObject.Find("AttachName").GetComponent<Text>();
            attachNameText.text = "" + selectedAttachment;
        }

        //When player is in gameplay (Playing a Level/Challenge)
        if (InGameplay) {
            timer += Time.deltaTime;

            //Check if player pauses game by pressing 'P'
            if (Input.GetKeyDown(KeyCode.P)) {
                if (Time.timeScale == 1.0f) {
                    Time.timeScale = 0.0f;
                    pauseMenu.SetActive(true);
                } else {
                    Time.timeScale = 1.0f;
                    pauseMenu.SetActive(false);
                }
            }

            //Check if max score was reached
            if (score >= scoreLimit) {
                //Display Completion Menu showing time and score
                Time.timeScale = 0.0f;
                finalStatusMenu.SetActive(true);
                finalTimeText = GameObject.Find("scoreStatusMenu/finalStatusMenu/finalTime").GetComponent<Text>();
                finalTimeText.text = "Final Time: " + timer.ToString("F2");
            }
        }
    }


    /*IsInGameplay()
    Desc: Gets and returns private InGameplay boolean varible.
    Parameters: None
    Return: Bool, InGameplay*/
    public bool IsInGameplay(){
        return InGameplay;
    }

    /*GetScore()
    Desc: Gets and returns private score variable.
    Parameters: None
    Return: Int, score*/
    public int GetScore() {
        return score;
    }

    /*SetScore(int)
    Desc: Sets the score varible to the value passed in.
    Parameters: int, newScore to be set
    Return: None*/
    public void SetScore(int newScore) {
        score = newScore;
    }

    /*SetScoreLimit(int)
    Desc: Set the max score limit to the value passed in. Some levels are completed when the score reaches the score limit.
    Parameters: int, newScoreLimit to be set to
    Return: None*/
    public void SetScoreLimit(int newScoreLimit) {
        scoreLimit = newScoreLimit;
    }

    /*AddToScore(int)
    Desc: Increments the score variable by the value passed in.
    Parameters: int, point to be added to score
    Return: None*/
    public void AddToScore(int point) {
        score += point;
    }

    /*ResetTimer()
    Desc: Resets the timer to 0.
    Parameters: None
    Return: None*/
    public void ResetTimer() {
        timer = 0;
    }

    /*SelectLevel(int)
    Desc: Sets the selectedLevel to the level value passed in.
    Parameters: int, level that is selected on the level select screen.
    Return: None*/
    public void SelectLevel(int level) {
        selectedLevel = level;
    }

    /*SetInCustomization(bool)
    Desc: Sets the InCustomization boolean variable to the value passed in.
    Parameters: bool, should be set to true when the player enters the customization screen, false otherwise.
    Return: None*/
    public void SetInCustomization(bool value) {
        InCustomization = value;
    }

    /*ResumeGame()
    Desc: Resumes the game by deactivating the pauseMenu and unfreezing time.
    Parameters: None
    Return: None*/
    public void ResumeGame() {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    /*FindSIMbot()
    Desc: Finds the SIMbot or SIMbotPython and it's attachmentSlot
    Parameters: None
    Return: True of False depending if it found the SIMbot*/
    public bool FindSIMbot() {
        //Find CarScript on SIMbot or SIMbotPython as well as it's associated gameObjects (Such as LED and AttachmentSlot).
        if(CarScript == null && GameObject.Find("SIMbot(Clone)") != null) {
            SIMbot = GameObject.Find("SIMbot(Clone)");
            CarScript = SIMbot.GetComponent<SimpleCarController>();
            AttachmentSlot = GameObject.Find("SIMbot(Clone)/SIMbot-Chassis/Attachments");
            LED = GameObject.Find("SIMbot(Clone)/PCBmodel/LED").GetComponent<Light>();
            Debug.Log("Found SIMbot");
            return true;
        } else if(CarScript == null && GameObject.Find("SIMbotPython(Clone)") != null) {
            SIMbot = GameObject.Find("SIMbotPython(Clone)");
            CarScript = SIMbot.GetComponent<SimpleCarController>();
            AttachmentSlot = GameObject.Find("SIMbotPython(Clone)/SIMbot-Chassis/Attachments");
            LED = GameObject.Find("SIMbotPython(Clone)/SIMbot-Chassis/PCBmodel/LED").GetComponent<Light>();
            Debug.Log("Found SIMbotPython");
            return true;
        } else {
            Debug.Log("Didn't find SIMbot");
            return false;
        }
    }

    /*ToggleLED()
    Desc: Toggles the LED light on the SIMbot on or off.
    Parameters: None
    Return: None*/
    public void ToggleLED() {
        if (LEDon) {
            LEDon = false;
        } else {
            LEDon = true;
        }

        //Check and update LED to be on or off accordingly
        CheckLED();
    }

    /*CheckLED()
    Desc: Checks LEDon variable and turns it on or off accordingly.
    Parameters: None
    Return: None*/
    private void CheckLED() {
        if (LEDon) {
            LED.enabled = true;
        } else {
            LED.enabled = false;
        }
    }

    /*GetTankControls()
    Desc: Returns the tankControls variable. True meaning each wheel has a forwards and backwards button (W/S & I/K).
    Parameters: None
    Return: Bool, tankControls value if tankControls are turned on or off*/
    public bool GetTankControls() {
        return tankControls;
    }

    /*ToggleTankControls()
    Desc: Toggles the controls for the SIMbot. Tank controls use the W/S & I/K keys for each wheal track. 
        If tankControls are off/false, then player dries with the AWSD or arrow keys.
    Parameters: None
    Return: None*/
    public void ToggleTankControls() {
        if (tankControls) {
            tankControls = false;
        } else {
            tankControls = true;
        }

        //Update the SIMbots tankControls bool on its script
        if(CarScript != null) {
            CarScript.tankControls = tankControls;
        }
    }

    /*TogglePythonontrols()
    Desc: Toggles the use of the Python scripts to control the SIMbot as opposed to user input via the keyboard.
        If pythonControls are set to true/on, then this overrides user controls (Keyboard input doesn't drive SIMbot).
    Parameters: None
    Return: None*/
    public void TogglePythonControls() {
        if (pythonControls) {
            pythonControls = false;
        } else {
            pythonControls = true;
        }
    }

    /*QuitGame()
    Desc: Quits the games application. (Only works when running the standalone executable)
    Parameters: None
    Return: None*/
    public void QuitGame() {
        Application.Quit();
    }
    
    /*UnloadAllScenes()
    Desc: Unloads all scenes except the GameManager. This is called everytime before another level is loaded.
    Parameters: None
    Return: None*/
    public void UnloadAllScenes() {
        int countLoaded = SceneManager.sceneCount;
 
        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).name != "GameManager") {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }

        //Delete all SIMbots because a new SIMbot is spawned every time a new level is loaded.
        if(SIMbot) { DeleteSIMbot(); }

        //Set time back to normal (In case it was paused or frozen)
        Time.timeScale = 1.0f;
    }


    /*Do(string)
    Desc: This method is called from menu buttons. Onbutton clicks, it passes a string and if
        that matches a specific string like below, it will call the respective methods.
        This method could be optimized but for not it works and allows a button to call
        multiple methods as needed.
    Parameters: string, functionName to be called by menu button click.
    Return: None*/
    public void Do(string functionName) {
        if (functionName == "LoadPlayScene") {
            LoadPlayScene();
        } else if (functionName == "LoadMenuScene") {
            LoadMenuScene();
        } else if (functionName == "QuitGame") {
            QuitGame();
        } else if (functionName == "RestartScene") {
            RestartScene();
        }  else if (functionName == "ResumeGame") {
            ResumeGame();
        } else if (functionName == "SetAttachmentNext") {
            SetAttachment(1);
            AddSelectedAttachment();
        } else if (functionName == "SetAttachmentPrev") {
            SetAttachment(-1);
            AddSelectedAttachment();
        }   else if (functionName == "ToggleTankControls") {
            ToggleTankControls();
        } else if (functionName == "TogglePythonControls") {
            TogglePythonControls();
        } else if (functionName == "ToggleLED") {
            ToggleLED();
        } else if (IsDigitsOnly(functionName) == true) {    //If functionName is a number only
            SelectLevel((int.Parse(functionName)));
        }
    }

    /*IsDigitsOnly(string)
    Desc: Helper function to see if Do(string functinName) is passed a string with only digits. Used to load levels.
    Parameters: string, to check if it consists only of digits.
    Return: Bool, if string as digits only or not*/
    public bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsDigit(c)) { return false; }
        }
        return true;
    }

    /*LoadMenuScene()
    Desc: Loads the menu scene.
    Parameters: None
    Return: None*/
    public void LoadMenuScene() {
        UnloadAllScenes();
        SceneManager.LoadScene(menuScene, LoadSceneMode.Additive);

        //reset variables
        InGameplay = false;
        pythonControls = false;
        
        //Make sure time isn't frozen
        Time.timeScale = 1.0f;
    }

    /*SetAttachment()
    Desc: Increses or decreases the selectedAttachment based on the value sent in. Depending on
        the size of the attachmentsPrefabs list, this will rotate through each of the attachments
        in that list.
    Parameters: int, value to increase or decrease the selectedAttachment value by.
    Return: None*/
    public void SetAttachment(int SetAttachment) {
        if (SetAttachment >= 0) {
            selectedAttachment += SetAttachment;
            if (selectedAttachment > attachmentPrefabs.Length-1) {selectedAttachment = 0;}
        } else if(SetAttachment < 0) {
            selectedAttachment += SetAttachment;
            if (selectedAttachment < 0) {selectedAttachment = attachmentPrefabs.Length-1;}
        }
    }

    /*DeleteAllAttachments()
    Desc: Deletes all currently attached SIMbot attachments.
    Parameters: None
    Return: None*/
    private void DeleteAllAttachments() {
        foreach (Transform child in AttachmentSlot.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    /*AddSelectedAttachment()
    Desc: Adds the currently selected attachment to the SIMbot.
    Parameters: None
    Return: None*/
    public void AddSelectedAttachment() {
            //Remove all attachments so there are no duplicates
            DeleteAllAttachments();

            if(attachmentPrefabs[selectedAttachment] != null) {
                //Create attachment prefab at attachment slot position and rotation
                GameObject newAttachment = Instantiate(attachmentPrefabs[selectedAttachment], AttachmentSlot.transform.position, AttachmentSlot.transform.rotation);

                //Make newAttachment a child of the attachmentSlot
                newAttachment.transform.parent = AttachmentSlot.transform;
            }
    }


    /*SpawnSIMbot()
    Desc: Spawns the SIMbot. Depending if python controls are enabled, will spawn the SIMbot with
        keyboard controls or python controls.
    Parameters: None
    Return: None*/
    private void SpawnSIMbot() {
        if (pythonControls) {
            //Spawn SIMbotPython
            Instantiate(SIMbotPythonPrefab, gameObject.transform.position, gameObject.transform.rotation);
            FindSIMbot();
        } else {
            //Spawn SIMbot that uses keyboard input
            Instantiate(SIMbotPrefab, gameObject.transform.position, gameObject.transform.rotation);
            FindSIMbot();
        }
    }

    /*DeleteSIMbot()
    Desc: Destroys the SIMbot gameObject.
    Parameters: None
    Return: None*/
    private void DeleteSIMbot() {
        GameObject.Destroy(SIMbot.gameObject);
    }

    /*LoadPlayScene()
    Desc: Loads a certain level based on the selectedLevel variable. Also sets score limmit and other values depending on the level.
    Parameters: None
    Return: None*/
    public void LoadPlayScene() {
        UnloadAllScenes();
        SetScore(0);
        ResetTimer();
        switch (selectedLevel) {
            case 1: 
                SceneManager.LoadScene(levelList[0], LoadSceneMode.Additive);
                SetScoreLimit(12);
                //StartCoroutine (LoadLevel(levelList[0]));
                //SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelList[0]));
                break;
            case 2:
                SceneManager.LoadScene(levelList[1], LoadSceneMode.Additive);
                SetScoreLimit(5);
                break;
            case 3:
                SceneManager.LoadScene(levelList[2], LoadSceneMode.Additive);
                break;
            default:
                Debug.Log("No Level Selected.");
                break;

        }

        InGameplay = true;

        SpawnSIMbot();

        //Make sure time isn't frozen
        Time.timeScale = 1.0f;
    }


    /*RestartScene()
    Desc: Finds the current scene/level. Unloads all scenes except the gameManager scene, and then reloads the current scene.
    Parameters: None
    Return: None*/
    public void RestartScene() {
        int countLoaded = SceneManager.sceneCount;
        string levelName = "";
 
        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).name != "GameManager") { levelName = SceneManager.GetSceneAt(i).name; }
        }
        UnloadAllScenes();
        SetScore(0);
        ResetTimer();
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);

        SpawnSIMbot();

        //Make sure time isn't frozen
        Time.timeScale = 1.0f;
    }
}