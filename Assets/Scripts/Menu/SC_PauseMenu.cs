using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_PauseMenu : MonoBehaviour
{
    /// <summary>Property <c>PauseMenu</c> references the <c>GameObject</c> PauseMenu in the scene.
    /// </summary>
    public GameObject PauseMenu;
    /// <summary>Property <c>OptionsMenu</c> references the <c>GameObject</c> OptionsMenu in the scene.
    /// </summary>
    public GameObject OptionsMenu;

    /// <summary>Property <c>tankControlsToggle</c> references the <c>Toggle</c> tankControlsToggle in the scene.
    /// </summary>
    public Toggle tankControlsToggle;
    /// <summary>Property <c>pythonToggle</c> references the <c>Toggle</c> pythonToggle in the scene.
    /// </summary>
    public Toggle pythonToggle;

    /// <summary>Field <c>levelManager</c> references the <c>LevelManager</c> script in the scene.
    /// </summary>
    private LevelManager levelManager;
    /// <summary>Field <c>saveManager</c> references the <c>SaveManager</c> script in the scene.
    /// </summary>
    private SaveManager saveManager;

    /// <summary>Field <c>SIMbot</c> references the <c>GameObject</c> SIMbot in the scene.
    /// </summary>
    private GameObject SIMbot;

    /// <summary>Field <c>SIMbotScript</c> references the <c><SIMbot</c> script in the scene.
    /// </summary>
    private SIMbot SIMbotScript;

    /// <summary>Field <c>eventSystem</c> references the <c>EventSystem</c> in the scene.
    /// </summary>
    private EventSystem eventSystem;

    /// <summary>Field <c>MAIN_MENU_LEVEL_NUMBER</c> holds the value for the index of the Main Menu scene.
    /// </summary>
    private int MAIN_MENU_LEVEL_NUMBER = 0;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();
        saveManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<SaveManager>();

        SIMbot = GameObject.FindGameObjectWithTag("Player");
        SIMbotScript = SIMbot.GetComponent<SIMbot>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();


        //determines whether or not the toggles are on by default according to what was selected on the bot before
        pythonToggle.isOn = SIMbotScript.pythonBot;
        tankControlsToggle.isOn = SIMbotScript.tankControls;
    }

    /// <summary>Method <c>MainMenuButton</c> saves the bot data, sets the selected level to the main menu,
    /// and loads that level. </summary>
    public void MainMenuButton()
    {
        //just in case, we'll save the bot's data
        SaveBotData();

        //set our level to the main menu
        levelManager.SetSelectedLevel(MAIN_MENU_LEVEL_NUMBER);

        //load the level
        levelManager.LoadLevel();
    }

    /// <summary>Method <c>PauseMenuButton</c> enables the <c>PauseMenu</c> and disables all other menus.</summary>
    public void PauseMenuButton()
    {
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    /// <summary>Method <c>OptionsMenuButton</c> enables the <c>OptionsMenu</c> and disables all other menus.</summary>
    public void OptionsMenuButton()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    /// <summary>Method <c>PauseGameToggle</c> Resumes the game if paused, pauses the game if not paused.</summary>
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

    /// <summary>Method <c>PauseGame</c> Pauses the game, disables camera orbiting, and enables the <c>PauseMenu</c></summary>
    private void PauseGame()
    {
        //pause the level
        levelManager.PauseLevel();

        //turn off the orbit script
        SIMbotScript.DisableCameraOrbit();

        //turn on the pause menu
        PauseMenuButton();
    }

    /// <summary>Method <c>ResumeGame</c> Resumes the game, enables camera orbiting, and disables the <c>PauseMenu</c></summary>
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

    /// <summary>Method <c>RestartLevel</c> Loads the scene the player is in again using <c>RestartLevel</c>
    /// in <c>levelManager</c>.</summary>
    /// <seealso cref="levelManager"/>
    public void RestartLevel()
    {
        levelManager.RestartLevel();
    }

    /// <summary>Method <c>SaveBotData</c> saves the SIMbot data using the <c>saveManager</c></summary>
    /// <seealso cref="saveManager"/>
    public void SaveBotData()
    {
        //update the SBdata on the bot with its current configuration
        SIMbotScript.SaveSIMbotOptions();

        //save that to a file
        saveManager.SaveSBData();
    }

    /// <summary>Method <c>SetBotTankControls</c> set the tank controls dependent on the toggle.</summary>
    /// <seealso cref="SIMbotScript"/>
    public void SetBotTankControls()
    {
        SIMbotScript.SetTankControls(tankControlsToggle.isOn);
    }

    /// <summary>Method <c>SetPythonBot</c> sets whether to use the python script or not dependent on the toggle.</summary>
    public void SetPythonBot()
    {
        SIMbotScript.SetPythonBot(pythonToggle.isOn);
    }

    /// <summary>Method <c>SetSelectedObject</c> sets the selected object in the <c>eventSystem</c>
    /// to the given object <c>obj</c>.</summary>
    /// <param><c>obj</c> is the object to be selected.</param>
    public void SetSelectedObject(GameObject obj)
    {
        eventSystem.SetSelectedGameObject(obj);
    }
}
