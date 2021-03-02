using System.Collections;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class PythonCommander : MonoBehaviour
{


    public string PythonCommands;
    public GameObject motors;
    public string SB = "SB.py";
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
        searchPaths.Add(Application.dataPath + @"\Plugins");
        engine.SetSearchPaths(searchPaths);
        engine.Runtime.LoadAssembly(Assembly.GetAssembly(typeof(GameObject)));


        string[] lines = File.ReadAllLines(Application.dataPath + "/Scripts/" + PythonCommands);
        var ScriptScope = engine.CreateScope();
        dynamic sbLib = engine.ExecuteFile(Application.dataPath + "/Scripts/" + SB);
        //Debug.Log("Hello! I am bad"); 

        dynamic sb = sbLib.SB(motors);
        ScriptScope.SetVariable("sb", sb);
        //Debug.Log("Hello! I am bad");


        foreach (string line in lines)
        {
            Debug.Log(line);
            var ScriptSource = engine.CreateScriptSourceFromString(line);
            ScriptSource.Execute(ScriptScope);
        }
        dynamic leftMotor = ScriptScope.GetVariable("leftMotor");
        Debug.Log(leftMotor);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
