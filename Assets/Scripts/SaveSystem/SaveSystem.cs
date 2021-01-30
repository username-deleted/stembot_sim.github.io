using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    //save the saveString data
    public static void Save(string saveString)
    {
        //directory does not exist?
        if (!Directory.Exists(SAVE_FOLDER))
        {
            //create save directory
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        else
        {
            //Debug.Log("Directory: \'" + SAVE_FOLDER + "\' Exists.");
        }
        File.WriteAllText(SAVE_FOLDER + "/data.txt", saveString);
    }

    //load the data
    public static string Load()
    {
        if(File.Exists(SAVE_FOLDER + "/data.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/data.txt");
            return saveString;
        }
        else
        {
            Debug.LogError("No such file exists...");
            return null;
        }
    }

    //delete all the data
    public static void DeleteData()
    {
        if (File.Exists(SAVE_FOLDER + "/data.txt"))
        {
            File.Delete(SAVE_FOLDER + "/data.txt");
            //delete the metadata for unity's editor system
            if (Application.isEditor)
            {
                File.Delete(SAVE_FOLDER + "/data.txt.meta");
            }
            Debug.Log("Data deleted!");
        }
    }
}
