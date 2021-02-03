using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    public LevelButtonObject levelButton;

    private int levelNumber;

    private LevelManager levelManager;
    private SC_MainMenu mainMenuScript;

    private Sprite levelSprite;
    private Image imageComponent;
    private Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        //get the level manager
        levelManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelManager>();

        //get the mainMenuScript
        mainMenuScript = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<SC_MainMenu>();

        //set the level number
        levelNumber = levelButton.levelNumber;
        //set the image sprite
        levelSprite = levelButton.levelImage;

        //create the image
        //CreateImageObject();
        imageComponent = gameObject.transform.Find("LevelImage").GetComponent<Image>();
        imageComponent.sprite = levelSprite;

        //create the text for the button
        //CreateTextObject();
        textComponent = gameObject.transform.Find("Text").GetComponent<Text>();
        textComponent.text = "Level " + levelNumber.ToString();
    }

    //set the levelManager to the correct level
    public void SetCurrentlySelectedLevel()
    {
        mainMenuScript.SetSelectedLevel(levelNumber);
        levelManager.SetSelectedLevel(levelNumber);
    }
}
