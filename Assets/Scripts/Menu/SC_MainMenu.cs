using System.Collections;
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

    private GameObject SIMbot;
    private SIMbot SIMbotScript;

    private LevelManager levelManager;

    public int currentlySelectedLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        SIMbot = GameObject.FindGameObjectWithTag("Player"); //get the SIMbot
        SIMbotScript = SIMbot.GetComponent<SIMbot>(); //get the SIMbot script
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>(); //get the level manager
    }

    public void ExitButton()
    {
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
