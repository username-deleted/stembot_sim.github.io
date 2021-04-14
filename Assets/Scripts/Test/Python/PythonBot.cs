using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class PythonBot : MonoBehaviour
{
    private List<Motor> motors = new List<Motor>();
    private List<SIMbotEvent> _events = new List<SIMbotEvent>();

    public event Action<int, float> OnSpeedChange;
    public event Action<float> OnTimeSleep; 

    private bool _waiting = false;

    public enum EventTypes
    {
        Speed,
        TimeSleep,
        MotorSleep,
        Distance,
        Null
    }

    public class SIMbotEvent
    {
        public EventTypes Action
        {
            get;
            set;
        }

        public SIMbotEvent()
        {
            Action = EventTypes.Null;
        }

        public override string ToString()
        {
            return "Action: " + Action;
        }
    }

    public class SIMbotSpeedEvent : SIMbotEvent
    {
        public float Speed
        {
            get;
            set;
        }

        public Motor WheelMotor
        {
            get;
            set;
        }

        public SIMbotSpeedEvent()
        {
            Action = EventTypes.Speed;
        }

        public override string ToString()
        {
            return "Action: " + Action + "\nSpeed: " + Speed + "\nMotor ID: " + WheelMotor.Id;
        }
    }

    public class SIMbotTimeSleepEvent : SIMbotEvent
    {

        /// <summary>
        /// Property <c>Duration</c> is the amount of time to wait before invoking the next event
        /// </summary>
        public float Duration
        {
            get;
            set;
        }

        public SIMbotTimeSleepEvent()
        {
            Action = EventTypes.TimeSleep;
        }
    }

    public class Motor
    {
        public int Id;
        private bool _sleeping;
        private bool _brakeMode;
        private float _motorSpeed;
        private GameObject _motor;
        private ArrayList _eventList;

        public Motor(int id)
        {
            Id = id;
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
        OnTimeSleep += SetWaitingTrue;
    }

    private void Update()
    {
        if (!_waiting && _events.Count > 0)
        {
            Invoke("ProcessNextSIMbotEvent", 0);
        }
    }

    private void SetWaitingTrue(float duration)
    {
        _waiting = true;
        Invoke("SetWaitingFalse", duration);
    }

    private void SetWaitingFalse()
    {
        _waiting = false;
    }

    private void ProcessNextSIMbotEvent()
    {
        //break out if no events to process
        if (_events.Count == 0)
        {
            return;
        }

        //get the next event
        var nextEvent = _events[0];
        //remove it from the list
        _events.RemoveAt(0);

        switch (nextEvent.Action)
        {
            //in the case of speed, variable 0 is the motor(Motor), variable 1 is the speed(float)
            case EventTypes.Speed:
                var speedEvent = (SIMbotSpeedEvent)nextEvent;
                Debug.Log("-- Speed Event --");
                Debug.Log("Motor ID: " + speedEvent.WheelMotor.Id);
                Debug.Log("Speed: " + speedEvent.Speed);

                //get the motor's id
                var motorId = speedEvent.WheelMotor.Id;

                //change the motor's speed
                motors[motorId - 1].speed(speedEvent.Speed);

                //throw the event to notify relevant scripts (car controller)
                OnSpeedChange?.Invoke(motorId, speedEvent.Speed);
                break;
            case EventTypes.TimeSleep:
                var timeSleepEvent = (SIMbotTimeSleepEvent) nextEvent;
                Debug.Log("-- Time Sleep Event --");
                Debug.Log("Duration: " + timeSleepEvent.Duration);

                OnTimeSleep?.Invoke(timeSleepEvent.Duration);
                break;
        }
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

        //Initialize sb python module with this script.
        dynamic sb = sbLib.SB(this);
        //Set it in the scope.
        scope.SetVariable("sb", sb);

        //same as before but with time module
        dynamic timeLib = engine.ExecuteFile(Application.dataPath + "/Scripts/Test/Python/time.py");
        dynamic time = timeLib.Time(this);
        scope.SetVariable("time", time);

        //Create a runnable script source from user script file.
        var ScriptSource = engine.CreateScriptSourceFromFile(Application.dataPath + "/Scripts/User/bot_test.py");
        //Execute said file.
        ScriptSource.Execute(scope);
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

    /// <summary>
    /// Method <c>GenerateSpeedEvent</c> creates a new SIMbotSpeedEvent with the given <c>motor</c> and <c>speed</c> parameters.
    /// </summary>
    /// <param name="motor"><c>motor</c> is the given motor of the SIMbot</param>
    /// <param name="speed"><c>speed</c> is the given speed of the motor</param>
    /// <returns>a new <c>SIMbotSpeedEvent</c></returns>
    public SIMbotEvent GenerateSpeedEvent(Motor motor, float speed)
    {
        var newEvent = new SIMbotSpeedEvent
        {
            Speed = speed,
            WheelMotor = motor
        };
        AddEventToEventList(newEvent);
        return newEvent;
    }

    public SIMbotEvent GenerateTimeSleepEvent(float duration)
    {
        var newEvent = new SIMbotTimeSleepEvent
        {
            Duration = duration
        };
        AddEventToEventList(newEvent);
        return newEvent;
    }

    private void AddEventToEventList(SIMbotEvent newEvent)
    {
        _events.Add(newEvent);
    }
}

