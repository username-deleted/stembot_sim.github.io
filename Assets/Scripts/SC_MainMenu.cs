﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject CustomizationMenu;
    public GameObject LevelSelectionMenu;

    public Text attachmentNumberText;

    public GameObject levelButtonsContainer;
    public LevelButtonObject[] levelButtons;

    public GameObject SIMbot;
    private SIMbot SIMbotScript;

    private LevelManager levelManager;

    public int currentlySelectedLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        SIMbotScript = SIMbot.GetComponent<SIMbot>();
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();

        //create a button for each of our levels
        foreach (LevelButtonObject levelButton in levelButtons)
        {
            //get the UI prefab
            GameObject buttonPrefab = levelButton.buttonPrefab;

            //create it
            GameObject button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);

            //set the information of the button on the button
            button.GetComponent<LevelButton>().levelButton = levelButton;

            //set the parent, in this case, the level buttons grid container
            button.transform.SetParent(levelButtonsContainer.transform);
        }
    }

    public void ExitButton()
    {
        // Debug.Log("Quit!!!");
        // Quit the game
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
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

    //set the currently selected level, called by the LevelButton script
    public void SetSelectedLevel(int levelNumber)
    {
        currentlySelectedLevel = levelNumber;
    }
}
