using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour
{

    private EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    //this is run when the script is enabled in the scene
    private void OnEnable()
    {
        if (eventSystem == null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }

        //set the currently selected object to this one.
        SetSelectedObject(gameObject);
    }

    public void SetSelectedObject(GameObject obj)
    {
        eventSystem.SetSelectedGameObject(obj);
    }
}
