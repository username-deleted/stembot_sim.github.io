using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Class <c>SC_MainMenu</c> controls all behavior related to the main menu.
/// </summary>
public class SC_MainMenu : MonoBehaviour
{

    /// <summary>Property <c>MainMenu</c> references the <c>GameObject</c> MainMenu in the scene.
    /// </summary>
    public GameObject MainMenu;
    /// <summary>Property <c>MainMenu</c> references the <c>GameObject</c> OptionsMenu in the scene.
    /// </summary>
    public GameObject OptionsMenu;
    /// <summary>Property <c>MainMenu</c> references the <c>GameObject</c> CustomizationMenu in the scene.
    /// </summary>
    public GameObject CustomizationMenu;
    /// <summary>Property <c>MainMenu</c> references the <c>GameObject</c> LevelSelectionMenu in the scene.
    /// </summary>
    public GameObject LevelSelectionMenu;

    /// <summary>Property <c>MainMenu</c> references the <c>Toggle</c> pythonToggle in the scene.
    /// </summary>
    public Toggle pythonToggle;
    /// <summary>Property <c>MainMenu</c> references the <c>Toggle</c> tankControlsToggle in the scene.
    /// </summary>
    public Toggle tankControlsToggle;

    /// <summary>Property <c>MainMenu</c> references the <c>Text</c> attachmentNumberText in the scene.
    /// </summary>
    public Text attachmentNumberText;
    /// <summary>Property <c>MainMenu</c> references the <c>Text</c> currentColorText in the scene.
    /// </summary>
    public Text currentColorText;

    /// <summary>Field <c>SIMbot</c> references the <c>GameObject</c> SIMbot in the scene.
    /// </summary>
    private GameObject SIMbot;

    /// <summary>Field <c>SIMbotScript</c> references the <c>SIMbot</c> script in the scene.
    /// </summary>
    private SIMbot SIMbotScript;
    /// <summary>Field <c>levelManager</c> references the <c>LevelManager</c> script in the scene.
    /// </summary>
    private LevelManager levelManager;
    /// <summary>Field <c>saveManager</c> references the <c>SaveManager</c> script in the scene.
    /// </summary>
    private SaveManager saveManager;

    /// <summary>Field <c>eventSystem</c> references the <c>EventSystem</c> in the scene.
    /// </summary>
    private EventSystem eventSystem;

    /// <summary>Property <c>currentlySelectedLevel</c> holds the value of the currently selected level.
    /// </summary>
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
        UpdateColorName();
    }

    /// <summary>This method fires when the exit button is clicked on the main menu.
    /// It exits the application.</summary>
    public void ExitButton()
    {
        // Quit the game
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    /// <summary>This method fires when the options button is clicked.
    /// It enables the options menu and disables all others.</summary>
    public void OptionsButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        CustomizationMenu.SetActive(false);
    }

    /// <summary>This method fires when the user navigates back from a submenu.
    /// It enables the main menu and disables all others.</summary>
    public void MainMenuButton()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(false);
    }

    /// <summary>This method fires when the customize button is clicked.
    /// It enables the customization menu and disables all others.</summary>
    public void CustomizationButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(true);
        UpdateAttachmentNumber();
    }

    /// <summary>This method fires when the play button is clicked.
    /// It enables the level select menu and disables all others.</summary>
    public void LevelSelectButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(true);
    }

    /// <summary>This method fires when the play button is clicked within the level select submenu.
    /// It disables all objects associated with the main menu and loads the currently selected level.</summary>
    public void PlayButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
        LevelSelectionMenu.SetActive(false);

        //load the currently selected level
        levelManager.LoadLevel();
    }

    /// <summary>This method updates the <c>attachmentNumberText</c> to the current <c>attachmentNumber</c>
    /// on the SIMbot.</summary>
    public void UpdateAttachmentNumber()
    {
        attachmentNumberText.text = SIMbotScript.attachmentNumber.ToString();
    }

    /// <summary>This method calls the <c>NextAttachment()</c> method to
    /// go to the next attachment on the <c>SIMbotScript</c>.</summary>
    public void NextAttachment()
    {
        SIMbotScript.NextAttachment();
        UpdateAttachmentNumber();
    }

    /// <summary>This method calls the <c>PreviousAttachment()</c> method to
    /// go to the previous attachment on the <c>SIMbotScript</c>.</summary>
    public void PreviousAttachment()
    {
        SIMbotScript.PreviousAttachment();
        UpdateAttachmentNumber();
    }

    /// <summary>This method updates the <c>currentColorText</c> to the current color
    /// on the <c>SIMbotScript</c>.</summary>
    public void UpdateColorName()
    {
        currentColorText.text = SIMbotScript.GetColor();
    }

    /// <summary>This method calls the <c>NextColor()</c> method to
    /// go to the next color on the <c>SIMbotScript</c>.</summary>
    public void NextColor()
    {
        SIMbotScript.NextColor();
        UpdateColorName();
    }

    /// <summary>This method calls the <c>PreviousColor()</c> method to
    /// go to the previous color on the <c>SIMbotScript</c>.</summary>
    public void PreviousColor()
    {
        SIMbotScript.PreviousColor();
        UpdateColorName();
    }

    /// <summary>This method calls the <c>ToggleLED()</c> method to
    /// toggle the LED on or off on the <c>SIMbotScript</c>.</summary>
    public void ToggleLED()
    {
        SIMbotScript.ToggleLED();
    }

    /// <summary>This method sets the currently selected level,
    /// called by the LevelButton script.</summary>
    /// <param><c>levelNumber</c> is the new level number.</param>
    public void SetSelectedLevel(int levelNumber)
    {
        currentlySelectedLevel = levelNumber;
    }

    /// <summary>This method calls <c>SaveSBData()</c> on <c>SIMbotScript</c> to save data
    /// to a file.</summary>
    public void SaveSBData()
    {
        saveManager.SaveSBData();
    }

    /// <summary>This method set the tank controls dependent on the toggle.</summary>
    public void SetBotTankControls()
    {
        SIMbotScript.SetTankControls(tankControlsToggle.isOn);
    }

    /// <summary>This method sets whether to use the python script or not dependent on the toggle.</summary>
    public void SetPythonBot()
    {
        SIMbotScript.SetPythonBot(pythonToggle.isOn);
    }

    /// <summary>This method sets the selected object in the <c>eventSystem</c>
    /// to the given object <c>obj</c>.</summary>
    /// <param><c>obj</c> is the object to be selected.</param>
    public void SetSelectedObject(GameObject obj)
    {
        eventSystem.SetSelectedGameObject(obj);
    }
}
