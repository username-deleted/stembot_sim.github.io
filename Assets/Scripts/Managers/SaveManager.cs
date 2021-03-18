using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    /// <summary>Instance variable <c>SIMbot</c> represents the
    /// SIMbot <c>GameObject<c> in the scene.</summary>
    private GameObject SIMbot;

    /// <summary>Instance variable <c>SIMbotScript</c> represents the
    /// SIMbot script <c>SIMbot<c> in the scene.</summary>
    private SIMbot SIMbotScript;

    private void Start()
    {
        SIMbot = GameObject.FindGameObjectWithTag("Player");
        SIMbotScript = SIMbot.GetComponent<SIMbot>();
    }

    /// <summary>Save the data on the SIMbot to a file.</summary>
    public void SaveSBData()
    {
        string saveString = JsonUtility.ToJson(SIMbotScript.SBData);
        SaveSystem.Save(saveString);
    }

    /// <summary>Load the SIMbot data from a file.</summary>
    public string LoadSBData()
    {
        return SaveSystem.Load();
    }

    /// <summary>Delete data on quit.</summary>
    private void OnApplicationQuit()
    {
        //we are not doing a persistent save system, delete the data so the app is fresh on start
        SaveSystem.DeleteData();
    }
}
