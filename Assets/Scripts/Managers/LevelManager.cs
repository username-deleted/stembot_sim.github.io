using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    /// <summary>Instance variable <c>selectedLevel</c> represents the
    /// currently selected level.</summary>
    public int selectedLevel = 1;

    /// <summary>Instance variable <c>sceneNames</c> represents the
    /// list of scene names.</summary>
    public string[] sceneNames;

    /// <summary>Instance variable <c>isPaused</c> whether the
    /// game is paused or not.</summary>
    public bool isPaused;

    /// <summary>Instance variable <c>MAIN_MENU_SCENE_NAME</c> the
    /// scene name of our main menu scene.</summary>
    private readonly string MAIN_MENU_SCENE_NAME = "FinalMainMenu";

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

    /// <summary>This method changes the point's location to
    /// the given coordinates.</summary>
    /// <param><c>levelNumber</c> is the new level number.</param>
    public void SetSelectedLevel(int levelNumber)
    {
        selectedLevel = levelNumber;
    }

    /// <summary>This method returns the selected level.</summary>
    /// <returns>The selected level</returns>
    public int GetSelectedLevel()
    {
        return selectedLevel;
    }

    /// <summary>This method sets the selected level to the given level number.</summary>
    /// <param><c>levelNumber</c> is the new level number.</param>
    public void LoadLevel(int levelNumber)
    {
        SetSelectedLevel(levelNumber);
        LoadLevel();
    }

    /// <summary>Load a scene with the current level number.</summary>
    public void LoadLevel()
    {
        //a scene number cannot be less than 0, or higher than the amount of levels we have
        if(selectedLevel > sceneNames.Length || selectedLevel < 0)
        {
            Debug.LogError("Selected level out of bounds");
            Debug.LogError("Trying to load Scene Number: " + selectedLevel + " but only have " + sceneNames.Length + " scenes...");
            return;
        }

        //if 0, set scene to main menu
        var scene = selectedLevel == 0 ? MAIN_MENU_SCENE_NAME : sceneNames[selectedLevel - 1];

        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }

    /// <summary>Pauses the level.</summary>
    public void PauseLevel()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>Resumes the level.</summary>
    public void ResumeLevel()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>Restarts the level.</summary>
    public void RestartLevel()
    {
        string nameOfCurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nameOfCurrentScene);
        Time.timeScale = 1;
    }

    /// <summary>Return the current scene's name</summary>
    /// <returns>The scene's name.</returns>
    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>Returns whether or not we are in the main menu scene.</summary>
    /// <returns>True if we're in the main menu scene; otherwise, false.</returns>
    public bool InMainMenuScene()
    {
        return SceneManager.GetActiveScene().name == MAIN_MENU_SCENE_NAME;
    }

}
