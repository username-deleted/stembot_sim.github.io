using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;

public class PythonExample : MonoBehaviour
{


    //public string pythonScript; 
    public string botScript;
    public GameObject motors;
    //public GameObject lights; 
    // Use this for initialization
    void Start()
    {
        var engine = Python.CreateEngine();
        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);

        //dynamic py = engine.ExecuteFile(Application.dataPath + @"/"+pythonScript);
        //This code was pointing to specific python code but after published, it was pointing outside the directory, adding "/Scripts/" to point back to the right place
        dynamic botObj = engine.ExecuteFile(Application.dataPath + @"/Scripts/" + botScript);
        dynamic STEMBot = botObj.botObj(motors);
        //Debug.Log(STEMBot.hello());
        //Debug.Log(greeter.random_number(1,5));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
