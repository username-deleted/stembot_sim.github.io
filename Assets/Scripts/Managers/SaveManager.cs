using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject SIMbot;
    private SIMbot SIMbotScript;

    private void Start()
    {
        SIMbotScript = SIMbot.GetComponent<SIMbot>();
    }

    //save the data on the simbot to a file
    public void SaveSBData()
    {
        string saveString = JsonUtility.ToJson(SIMbotScript.SBData);
        SaveSystem.Save(saveString);
    }

    private void OnApplicationQuit()
    {
        //we are not doing a persistent save system, delete the data so the app is fresh on start
        SaveSystem.DeleteData();
    }
}
