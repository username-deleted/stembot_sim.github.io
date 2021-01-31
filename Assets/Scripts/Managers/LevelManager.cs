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
            case 1:
                scene = sceneNames[0];
                break;

            case 2:
                scene = sceneNames[1];
                break;
            default:
                Debug.LogError("Level " + currentlySelectedLevel.ToString() + " does not exist.");
                break;
        }

        SceneManager.LoadScene(scene);
    }
}
