using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitButton()
    {
        // Debug.Log("Quit!!!");
        // Quit the game
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
