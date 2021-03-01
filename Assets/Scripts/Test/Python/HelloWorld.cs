using UnityEngine;
using IronPython.Hosting;
using System.Collections.Generic;

public class HelloWorld : MonoBehaviour
{
	void Start()
	{
        var engine = global::UnityPython.CreateEngine();
        var scope = engine.CreateScope();
        //scope.SetVariable();
        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);

        dynamic botObj = engine.ExecuteFile(Application.dataPath +"/Scripts/Test.py");
        dynamic simScript = botObj.Test(GameObject.FindGameObjectWithTag("Player"));
        simScript.helloUnity();
        simScript.getBotScript();

        //dynamic py = engine.ExecuteFile(Application.dataPath + @"/"+pythonScript);
        //This code was pointing to specific python code but after published, it was pointing outside the directory, adding "/Scripts/" to point back to the right place
        //dynamic botObj = engine.ExecuteFile(Application.dataPath + "/Scripts/" + botScript);
        //dynamic STEMBot = botObj.botObj(motors);
        //Debug.Log(STEMBot.hello());
        //Debug.Log(greeter.random_number(1,5));
    }
}