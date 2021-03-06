using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private const string filename = "/data.txt"; //where to save and load the data from
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/"; //the folder to save/load from

    //save the saveString data
    /// <summary>This method saves the given string to a file</summary>
    /// <param><c>saveString</c> is the string to save.</param>
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
        File.WriteAllText(SAVE_FOLDER + filename, saveString);
    }

    /// <summary>This method loads previously saved data from the file. </summary>
    /// <returns>The data string</returns>
    /// <see cref="filename"/>
    /// <see cref="SAVE_FOLDER"/>
    public static string Load()
    {
        if(File.Exists(SAVE_FOLDER + filename))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + filename);
            return saveString;
        }
        else
        {
            Debug.LogError("No such file exists...");
            return null;
        }
    }

    /// <summary>This method deletes all saved data</summary>
    public static void DeleteData()
    {
        if (File.Exists(SAVE_FOLDER + filename))
        {
            File.Delete(SAVE_FOLDER + filename);
            //delete the metadata for unity's editor system
            if (Application.isEditor)
            {
                File.Delete(SAVE_FOLDER + filename + ".meta");
            }
            //Debug.Log("Data deleted!");
        }
    }
}
