using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public string menuScene;
    public string levelSelectScene;

    public GameObject SIMbotPrefab;
    public GameObject SIMbotPythonPrefab;

    private GameObject SIMbot;
    private SimpleCarController CarScript;

    public GameObject[] attachmentPrefabs;

    //private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        //Find SIMbot & related gameObjects
        if (CarScript == null) {
            FindSIMbot();
        }
    }

    /*FindSIMbot()
    Desc: Finds the SIMbot or SIMbotPython and it's attachmentSlot
    Parameters: None
    Return: True of False depending if it found the SIMbot*/
    public bool FindSIMbot() {
        //Find CarScript on SIMbot or SIMbotPython as well as it's associated gameObjects (Such as LED and AttachmentSlot).
        if(CarScript == null && GameObject.Find("SIMbot(Clone)") != null) {
            SIMbot = GameObject.Find("SIMbot(Clone)");
            CarScript = SIMbot.GetComponent<SimpleCarController>();
            Debug.Log("Found SIMbot");
            return true;
        } else if(CarScript == null && GameObject.Find("SIMbotPython(Clone)") != null) {
            SIMbot = GameObject.Find("SIMbotPython(Clone)");
            CarScript = SIMbot.GetComponent<SimpleCarController>();
            Debug.Log("Found SIMbotPython");
            return true;
        } else {
            Debug.Log("Didn't find SIMbot");
            return false;
        }
    }
}