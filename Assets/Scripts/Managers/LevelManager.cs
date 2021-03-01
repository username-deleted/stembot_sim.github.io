using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int selectedLevel = 1; //the currently selected level
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
        selectedLevel = levelNumber;
    }

    public int GetSelectedLevel()
    {
        return selectedLevel;
    }

    //load a level with the passed in level number
    public void LoadLevel(int levelNumber)
    {
        SetSelectedLevel(levelNumber);
        LoadLevel();
    }

    //load a scene with the current level number
    public void LoadLevel()
    {
        //a scene number cannot be less than 0, or higher than the amount of levels we have
        if(selectedLevel > sceneNames.Length || selectedLevel < 0)
        {
            Debug.LogError("Selected level out of bounds");
            Debug.LogError("Trying to load Scene Number: " + selectedLevel + " but only have " + sceneNames.Length + " scenes...");
            return;
        }

        string scene;
        //if 0, set scene to main menu
        if(selectedLevel == 0)
        {
            scene = MAIN_MENU_SCENE_NAME;
        }
        //else set scene to corresponding name in array
        else
        {
            scene = sceneNames[selectedLevel - 1];
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

    public void RestartLevel()
    {
        string nameOfCurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nameOfCurrentScene);
        Time.timeScale = 1;
    }
}
