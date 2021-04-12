using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourManager : MonoBehaviour
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
    private void Awake()
    {
        completionMenuTimeText = GameObject.FindGameObjectWithTag("CompletionMenuTimeText").GetComponent<Text>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SIMBotCollider")
        {
            completionMenu.enableCompletionMenu();
            completionMenuTimeText.text = "Time: " + timeManager.getTime().ToString("F2");
        }
    }
}
