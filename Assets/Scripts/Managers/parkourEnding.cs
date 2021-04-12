using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parkourEnding : MonoBehaviour
{
    private SC_CompletionMenu completionMenu;
    private TimeManager timeManager;
    private Text completionMenuTimeText;
    // Start is called before the first frame update
    void Start()
    {
        completionMenu = GameObject.FindGameObjectWithTag("CompletionCanvas").GetComponent<SC_CompletionMenu>();
        timeManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TimeManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "SIMbot 3-5-21")
        {
            completionMenu.enableCompletionMenu();
            completionMenuTimeText.text = timeManager.getTime().ToString("F2");
        }
    }
}
