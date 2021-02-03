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


        //determines whether or not he toggles are on by default according to what was selected on the bot before
        pythonToggle.isOn = SIMbotScript.pythonBot;
        tankControlsToggle.isOn = SIMbotScript.tankControls;
    }

    //sets the level to the main menu, then loads it
    public void MainMenuButton()
    {
        levelManager.SetSelectedLevel(MAIN_MENU_LEVEL_NUMBER);
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

    public void PauseGame()
    {
        levelManager.PauseLevel();
        PauseMenuButton();
    }

    public void ResumeGame()
    {
        //resume the level
        levelManager.ResumeLevel();
    }

    public void SaveBotData()
    {
        //update the SBdata on the bot with its current configuration
        SIMbotScript.SaveSIMbotOptions();

        //save that to a file
        saveManager.SaveSBData();
    }
}
