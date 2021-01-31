using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    public LevelButtonObject levelButton;

    public int levelNumber;

    //size in pixels
    public int spriteSize = 225;

    //text Y offset
    public int textYOffset = 160;

    private LevelManager levelManager;
    private SC_MainMenu mainMenuScript;

    private Sprite levelSprite;
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
        CreateImageObject();

        //create the text for the button
        CreateTextObject();

        //listen for a click and set the level when we hear one
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { SetCurrentlySelectedLevel(); });
    }

    private void CreateImageObject()
    {
        GameObject imageObject = new GameObject();
        imageObject.AddComponent<RectTransform>().sizeDelta = new Vector2(spriteSize, spriteSize);
        imageObject.AddComponent<Image>().sprite = levelSprite;

        //set the parent
        imageObject.transform.SetParent(gameObject.transform, false);
        //name it
        imageObject.name = "Level " + levelNumber.ToString() + " Image";

        //instantiate the image
        GameObject obj = Instantiate(imageObject, Vector3.zero, Quaternion.identity);
    }

    private void CreateTextObject()
    {
        //create the text for the button
        GameObject textObject = new GameObject();

        //set the position
        textObject.AddComponent<RectTransform>().SetPositionAndRotation(new Vector3(0, textYOffset, 1), Quaternion.identity);

        RectTransform rt = textObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 100);

        //set the text
        textObject.AddComponent<Text>().text = "Level " + levelNumber.ToString();

        //get the text component
        var text = textObject.GetComponent<Text>();

        //set the font/material
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.fontSize = 36;
        text.material = ArialFont.material;
        text.alignment = TextAnchor.MiddleCenter;

        //set the parent
        textObject.transform.SetParent(gameObject.transform, false);

        //name it
        textObject.name = "Level " + levelNumber.ToString() + " Text";

        //instantiate the text
        Instantiate(textObject, Vector3.zero, Quaternion.identity);
    }

    //set the levelManager to the correct level
    public void SetCurrentlySelectedLevel()
    {
        mainMenuScript.SetSelectedLevel(levelNumber);
        levelManager.SetSelectedLevel(levelNumber);
    }
}
