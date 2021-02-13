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
            //CheckLED();
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

    /*DeleteAllAttachments()
    Desc: Deletes all currently attached SIMbot attachments.
    Parameters: None
    Return: None*/
    private void DeleteAllAttachments() {
        foreach (Transform child in AttachmentSlot.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    /*DeleteSIMbot()
    Desc: Destroys the SIMbot gameObject.
    Parameters: None
    Return: None*/
    private void DeleteSIMbot() {
        GameObject.Destroy(SIMbot.gameObject);
    }
}