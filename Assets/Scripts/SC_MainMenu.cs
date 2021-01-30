using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject CustomizationMenu;

    public Text attachmentNumberText;

    public GameObject SIMbot;
    private SIMbot SIMbotScript;
    // Start is called before the first frame update
    void Start()
    {
        SIMbotScript = SIMbot.GetComponent<SIMbot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAttachmentNumber()
    {
        Debug.Log("UPDATE!!!");
        if (attachmentNumberText)
        {
            Debug.Log("I exist...");
        }
        attachmentNumberText.text = SIMbotScript.attachmentNumber.ToString();
    }

    public void ExitButton()
    {
        // Debug.Log("Quit!!!");
        // Quit the game
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OptionsButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        CustomizationMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(false);
    }

    public void CustomizationButton()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CustomizationMenu.SetActive(true);
    }
}
