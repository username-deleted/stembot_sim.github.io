using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class PythonBot : MonoBehaviour
{
    private List<Motor> motors = new List<Motor>();
    private bool hasRan = false;
    private List<SIMbotEvent> _events;

    public class SIMbotEvent
    {
        private string _action;
        private ArrayList _variables;

        public SIMbotEvent(string action)
        {
            _action = action;
        }

        public void SetupVariables(ArrayList variables)
        {
            _variables = variables;
        }
    }

    public class Motor
    {
        private int _id;
        private bool _sleeping;
        private bool _brakeMode;
        private float _motorSpeed;
        private GameObject _motor;
        private ArrayList _eventList;

        public Motor(int id)
        {
            _id = id;
            _sleeping = false;
            _brakeMode = true;
            _motorSpeed = 0;
            _motor = GameObject.FindGameObjectWithTag("Wheels").transform.GetChild(id - 1).gameObject;
        }

        public bool sleep()
        {
            Debug.Log("Are you sleeping?");
            return _sleeping;
        }
        public void sleep(bool sleeping)
        {
            Debug.Log("Change Sleep!");
            _sleeping = sleeping;
        }

        public float speed()
        {
            return _motorSpeed;
        }

        public void speed(float x)
        {
            _motorSpeed = x;
            Debug.Log("Speed Changed to " + _motorSpeed);
            
        }
    }

    // Start is called before the first frame update
    private void Start()
    { 
        RunPythonScript();
    }

    private void Update()
    { 
        /*if (!hasRan)
        {
            Debug.Log("Run it.");
            hasRan = true;
            Invoke("RunPythonScript", 5);
            
        }*/
    }

    

    private void RunPythonScript()
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

        //Create our scope.
        var scope = engine.CreateScope();
        //Execute sb python module.
        dynamic sbLib = engine.ExecuteFile(Application.dataPath + "/Scripts/Test/Python/sb.py");

        //Initialize sb python module.
        dynamic sb = sbLib.SB(this);
        //Set it in the scope.
        scope.SetVariable("sb", sb);

        //Create a runnable script source from user script file.
        var ScriptSource = engine.CreateScriptSourceFromFile(Application.dataPath + "/Scripts/User/bot_test.py");
        //Execute said file.
        ScriptSource.Execute(scope);


        //dynamic leftMotor = scope.GetVariable("motor_1");
        //Debug.Log(leftMotor);
    }

    public void HelloPython()
    {
        Debug.Log("This method was called from python!");
    }

    //this should be called from python script to create the motors
    public Motor CreateMotor(int id)
    {
        var newMotor = new Motor(id);
        motors.Add(newMotor);
        return newMotor;
    }

    public SIMbotEvent CreateSIMbotEvent(string action)
    {
        return new SIMbotEvent(action);
    }

    //Due to the communication between c sharp and python, generic methods could not be made. One reason for this is
    //the fact that each method generates an event with varying amount of variables. Passing an array from python to c#
    //might cause some issues. Investigating...
    public SIMbotEvent GenerateSpeedEvent(float speed)
    {
        var newEvent = new SIMbotEvent("speed");
        var temp = new ArrayList
        {
            speed
        };
        SetupVariablesAndAddToEventList(newEvent, temp);
        return newEvent;
    }

    public SIMbotEvent GenerateSleepEvent(float duration)
    {
        var newEvent = new SIMbotEvent("sleep");
        var temp = new ArrayList
        {
            duration
        };
        SetupVariablesAndAddToEventList(newEvent, temp);
        return newEvent;
    }

    private void AddEventToEventList(SIMbotEvent newEvent)
    {
        _events.Add(newEvent);
    }

    private void SetupVariablesAndAddToEventList(SIMbotEvent newEvent, ArrayList temp)
    {
        newEvent.SetupVariables(temp);
        AddEventToEventList(newEvent);
    }
}

