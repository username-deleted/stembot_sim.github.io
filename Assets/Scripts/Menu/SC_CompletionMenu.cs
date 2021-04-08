using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_CompletionMenu : MonoBehaviour
{
    private LevelManager levelManager;
    public GameObject CompletionMenu;
    private SIMbot SIMbotScript;
    private GameObject SIMbot;

    //the level number of the main menu, 0
    private int MAIN_MENU_LEVEL_NUMBER = 0;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();
        SIMbot = GameObject.FindGameObjectWithTag("Player");
        SIMbotScript = SIMbot.GetComponent<SIMbot>();
        CompletionMenu.SetActive(false);
    }

    //sets the level to the main menu, then loads it
    public void MainMenuButton()
    {
        //set our level to the main menu
        levelManager.SetSelectedLevel(MAIN_MENU_LEVEL_NUMBER);

        //load the level
        levelManager.LoadLevel();
    }
    
    //called on clicking the restart level button in the pause menu
    public void RestartLevel()
    {
        levelManager.RestartLevel();
    }

    public void ExitButton()
    {
        // Quit the game
        Application.Quit();
    }

    //enable completion menu
    public void enableCompletionMenu()
    {
        //pause the level
        levelManager.PauseLevel();

        //turn off the orbit script
        SIMbotScript.DisableCameraOrbit();

        CompletionMenu.SetActive(true);
    }
}
