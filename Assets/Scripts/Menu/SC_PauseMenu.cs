using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PauseMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionsMenu;

    public Toggle tankControlsToggle;
    public Toggle pythonToggle;

    private LevelManager levelManager;
    private SaveManager saveManager;

    private GameObject SIMbot;
    private SIMbot SIMbotScript;

    //the level number of the main menu, 0
    private int MAIN_MENU_LEVEL_NUMBER = 0;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();
        saveManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SaveManager>();

        SIMbot = GameObject.FindGameObjectWithTag("Player");
        SIMbotScript = SIMbot.GetComponent<SIMbot>();

        //determines whether or not the toggles are on by default according to what was selected on the bot before
        pythonToggle.isOn = SIMbotScript.pythonBot;
        tankControlsToggle.isOn = SIMbotScript.tankControls;
    }

    //sets the level to the main menu, then loads it
    public void MainMenuButton()
    {
        //just in case, we'll save the bot's data
        SaveBotData();

        //set our level to the main menu
        levelManager.SetSelectedLevel(MAIN_MENU_LEVEL_NUMBER);

        //load the level
        levelManager.LoadLevel();
    }

    //enable pause menu
    public void PauseMenuButton()
    {
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    //enable options menu
    public void OptionsMenuButton()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    //toggle whether game is paused or not
    public void PauseGameToggle()
    {
        //if the game is paused, un pause
        if (levelManager.isPaused)
        {
            ResumeGame();
        }
        else
        //if the game isn't paused, pause it
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        //pause the level
        levelManager.PauseLevel();

        //turn off the orbit script
        SIMbotScript.DisableCameraOrbit();

        //turn on the pause menu
        PauseMenuButton();
    }

    public void ResumeGame()
    {
        //resume the level
        levelManager.ResumeLevel();

        //turn on the orbit script
        SIMbotScript.EnableCameraOrbit();

        //turn off the menus
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    //called on clicking the restart level button in the pause menu
    public void RestartLevel()
    {
        levelManager.RestartLevel();
    }

    public void SaveBotData()
    {
        //update the SBdata on the bot with its current configuration
        SIMbotScript.SaveSIMbotOptions();

        //save that to a file
        saveManager.SaveSBData();
    }

    //set the controls dependent on the toggle
    public void SetBotTankControls()
    {
        SIMbotScript.SetTankControls(tankControlsToggle.isOn);
    }

    //set whether to use th python script or not dependent on the toggle
    public void SetPythonBot()
    {
        SIMbotScript.SetPythonBot(pythonToggle.isOn);
    }
}
