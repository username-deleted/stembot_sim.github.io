using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonBehaviour : MonoBehaviour
{
    public string functionName;
    public GameManagerBehaviour sceneManager;

    void Start() {
        sceneManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    /*OnButtonClick()
    Desc: When the button is clicked, this method is called (Has to be set in the inspector).
        This method then calls the GameManger method 'DO()' and sends in a string functionName
        that the Gamemanager then handles.
    Parameters: None
    Return: None*/
    public void OnButtonClick() {
        sceneManager.Do(functionName);
    }

    /*SetInCustomization(bool)
    Desc: This method calls the gameManager method to set the bool InCustomization to the
        value passed into this method. 
        (This is an example of another way the button behaviours could be handled as
        opposed to the 'DO()' function inside the gameManager. They do however work
        the same)
    Parameters: bool, the value to set InCustomization to. Should be set to True when
        the player enters the customization screen at the menu.
    Return: None*/
    public void SetInCustomization(bool value) {
        sceneManager.SetInCustomization(value);
    }

}