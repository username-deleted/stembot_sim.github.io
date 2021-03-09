﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject CustomizationMenu;
    public GameObject LevelSelectionMenu;
    public Toggle pythonToggle;
    public Toggle tankControlsToggle;

    public Text attachmentNumberText;

    private GameObject SIMbot;
    private SIMbot SIMbotScript;

    private LevelManager levelManager;
    private SaveManager saveManager;

    private EventSystem eventSystem;

    public int currentlySelectedLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        SIMbot = GameObject.FindGameObjectWithTag("Player"); //get the SIMbot
        SIMbotScript = SIMbot.GetComponent<SIMbot>(); //get the SIMbot script
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>(); //get the level manager
        saveManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SaveManager>(); //get the save manager

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();

        //determines whether or not the toggles are on by default according to what was selected on the bot before
        //useful for when the user backs out of the game, but does not quit
        pythonToggle.isOn = SIMbotScript.pythonBot;
        tankControlsToggle.isOn = SIMbotScript.tankControls;

        //update the attachment number at start
        //used mainly for when the player comes back from a level
        UpdateAttachmentNumber();
    }

    public void ExitButton()
    {
        // Quit the game
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OptionsButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        CustomizationMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(false);
    }

    public void CustomizationButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(true);
        UpdateAttachmentNumber();
    }

    public void LevelSelectButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(true);
    }

    public void PlayButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(false);

        //load the currently selected level
        levelManager.LoadLevel();
    }

    //update the attachment number display
    public void UpdateAttachmentNumber()
    {
        attachmentNumberText.text = SIMbotScript.attachmentNumber.ToString();
    }

    public void NextAttachment()
    {
        SIMbotScript.NextAttachment();
        UpdateAttachmentNumber();
    }

    public void PreviousAttachment()
    {
        SIMbotScript.PreviousAttachment();
        UpdateAttachmentNumber();
    }

    public void ToggleLED()
    {
        SIMbotScript.ToggleLED();
    }

    //set the currently selected level, called by the LevelButton script
    public void SetSelectedLevel(int levelNumber)
    {
        currentlySelectedLevel = levelNumber;
    }

    //called on the play and back buttons on the level select menu
    public void SaveSBData()
    {
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

    public void SetSelectedObject(GameObject obj)
    {
        eventSystem.SetSelectedGameObject(obj);
    }
}
