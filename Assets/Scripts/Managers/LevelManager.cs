using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int currentlySelectedLevel = 1;
    public string[] sceneNames;
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
                scene = "FinalMainMenu";
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
    }


    //pause the level
    public void PauseLevel()
    {
        Time.timeScale = 0;
    }

    public void ResumeLevel()
    {
        Time.timeScale = 1;
    }
}
