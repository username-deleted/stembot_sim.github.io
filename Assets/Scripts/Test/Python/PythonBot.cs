using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

public class PythonBot : MonoBehaviour
{
    public GameObject motors;

    // Start is called before the first frame update
    void Start()
    {
        var engine = global::UnityPython.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of PythonBot.py
        searchPaths.Add(Application.dataPath + "/Scripts/Test/Python");
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        searchPaths.Add(Application.dataPath + @"\Plugins");
        engine.SetSearchPaths(searchPaths);
        engine.Runtime.LoadAssembly(Assembly.GetAssembly(typeof(GameObject)));
        //var scope = engine.CreateScope();

        
        //string code = "test = None\n";
        //engine.CreateScriptSourceFromString(code);
        //scope.SetVariable("test", this);
        //dynamic sbLib = engine.ExecuteFile(Application.dataPath + "/Scripts/Test/Python/sb.py");
        //var CodeToexec = engine.CreateScriptSourceFromFile(Application.dataPath + "/Scripts/Test/Python/sb.py");
        //dynamic sbLib = CodeToexec.Execute(scope);
        //scope.SetVariable("motor_1", sbLib);

        //dynamic motor_1 = sbLib.Motor(1, this);
        //Debug.Log(motor_1.helloWorld());


        string[] lines = File.ReadAllLines(Application.dataPath + "/Scripts/Test/Python/bot_test.py");
        var scope = engine.CreateScope();
        dynamic sbLib = engine.ExecuteFile(Application.dataPath + "/Scripts/Test/Python/sb.py");
        //Debug.Log("Hello! I am bad"); 

        dynamic sb = sbLib.SB(motors);
        scope.SetVariable("sb", sb);
        //Debug.Log("Hello! I am bad");


        foreach (string line in lines)
        {
            Debug.Log(line);
            var ScriptSource = engine.CreateScriptSourceFromString(line);
            ScriptSource.Execute(scope);
        }
        dynamic leftMotor = scope.GetVariable("motor_1");
        Debug.Log(leftMotor);

    }

    public void HelloPython()
    {
        Debug.Log("This method was called from python!");
    }
}
