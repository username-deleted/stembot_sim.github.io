using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int currentlySelectedLevel = 1; //the currently selected level
    public string[] sceneNames; //the list of scene names
    public bool isPaused; //whether the game is paused or not
    private readonly string MAIN_MENU_SCENE_NAME = "FinalMainMenu"; //the scene name of our main menu scene

    private void Start()
    {
        //if we're not in the main menu, start with the cursor locked
        if(SceneManager.GetActiveScene().name != MAIN_MENU_SCENE_NAME)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void SetSelectedLevel(int levelNumber)
    {
        currentlySelectedLevel = levelNumber;
    }

    //load a scene with the given level number
    public void LoadLevel()
    {
        string scene = "";
        switch (currentlySelectedLevel)
        {
            //load the main menu
            case 0:
                scene = MAIN_MENU_SCENE_NAME;
                break;

            //load level 1
            case 1:
                scene = sceneNames[0];
                break;

            //load level 2
            case 2:
                scene = sceneNames[1];
                break;
            default:
                Debug.LogError("Level " + currentlySelectedLevel.ToString() + " does not exist.");
                break;
        }

        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }


    //pause the level
    public void PauseLevel()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    //resume the level
    public void ResumeLevel()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //return the current scene's level name
    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    //return whether or not we are in the main menu scene
    public bool InMainMenuScene()
    {
        if(SceneManager.GetActiveScene().name == MAIN_MENU_SCENE_NAME)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
