using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class PythonBot : MonoBehaviour
{
    /// <summary>
    /// Field <c>motors</c> holds a list of Motor objects, this should be the two motors on the SIMbot.
    /// </summary>
    private List<Motor> motors = new List<Motor>();
    /// <summary>
    /// Field <c>_events</c> is a queue in which events are loaded into and pulled off from in Update
    /// </summary>
    private List<SIMbotEvent> _events = new List<SIMbotEvent>();

    /// <summary>
    /// Property <c>OnSpeedChange</c> is a C# event that is invoked on speed changes
    /// </summary>
    public event Action<int, float> OnSpeedChange;
    /// <summary>
    /// Property <c>OnTimeSleep</c> is a C# event that is invoked on time.sleep calls
    /// </summary>
    public event Action<float> OnTimeSleep; 

    /// <summary>
    /// Field <c>_waiting</c> is whether or not the events are being process. If true, no events will be processed.
    /// </summary>
    private bool _waiting = false;

    /// <summary>
    /// Property <c>EventTypes</c> is an enum that holds all possible events for the SIMbot.
    /// </summary>
    public enum EventTypes
    {
        Speed,
        TimeSleep,
        MotorSleep,
        Distance,
        Null
    }

    /// <summary>
    /// Class <c>SIMbotEvent</c> is the main class that all event types will inherit. It describes what a SIMbot event is.
    /// </summary>
    public class SIMbotEvent
    {
        /// <summary>
        /// Property <c>Action</c> is the action type of the event.
        /// </summary>
        public EventTypes Action
        {
            get;
            set;
        }

        /// <summary>
        /// The Constructor, defaults to <c>EventTypes.Null</c>.
        /// </summary>
        public SIMbotEvent()
        {
            Action = EventTypes.Null;
        }

        /// <summary>
        /// Returns a string representation of the event
        /// </summary>
        /// <returns>A string representation of the event</returns>
        public override string ToString()
        {
            return "Action: " + Action;
        }
    }

    /// <summary>
    /// Class <c>SIMbotSpeedEvent</c> inherits <c>SIMbotEvent</c> and describes a speed event.
    /// </summary>
    public class SIMbotSpeedEvent : SIMbotEvent
    {
        /// <summary>
        /// Property <c>Speed</c> is the speed the motor should be set to.
        /// </summary>
        public float Speed
        {
            get;
            set;
        }

        /// <summary>
        /// Property <c>WheelMotor</c> is the motor to apply the speed to.
        /// </summary>
        public Motor WheelMotor
        {
            get;
            set;
        }

        /// <summary>
        /// The Constructor sets the <c>Action</c> to <c>EventTypes.Speed</c>.
        /// </summary>
        public SIMbotSpeedEvent()
        {
            Action = EventTypes.Speed;
        }

        /// <summary>
        /// Returns a string representation of the event
        /// </summary>
        /// <returns>A string representation of the event</returns>
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

        /// <summary>
        /// The Constructor sets the <c>Action</c> to <c>EventTypes.TimeSleep</c>.
        /// </summary>
        public SIMbotTimeSleepEvent()
        {
            Action = EventTypes.TimeSleep;
        }
    }

    /// <summary>
    /// Class <c>Motor</c> represents a SIMbot motor.
    /// </summary>
    public class Motor
    {
        /// <summary>
        /// Property <c>Id</c> is the motor Id.
        /// </summary>
        public int Id;
        /// <summary>
        /// Field <c>_sleeping</c> is whether the motor is sleeping.
        /// </summary>
        private bool _sleeping;
        /// <summary>
        /// Field <c>_brakeMode</c> is the brake mode of the motor.
        /// </summary>
        private bool _brakeMode;
        /// <summary>
        /// Field <c>_motorSpeed</c> is the speed of the motor.
        /// </summary>
        private float _motorSpeed;
        /// <summary>
        /// field <c>_motor</c> is the GameObject within the scene that represents the motor.
        /// </summary>
        private GameObject _motor;

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

    /// <summary>
    /// Method <c>SetWaitingTrue</c> sets <c>_waiting</c> to true for a set duration, invokes SetWaitingFalse after <c>duration</c>.
    /// </summary>
    /// <param name="duration">Parameter <c>duration</c> is the time to wait</param>
    private void SetWaitingTrue(float duration)
    {
        _waiting = true;
        Invoke("SetWaitingFalse", duration);
    }

    /// <summary>
    /// Method <c>SetWaitingFalse</c> sets <c>_waiting</c> to false.
    /// </summary>
    private void SetWaitingFalse()
    {
        _waiting = false;
    }

    /// <summary>
    /// Method <c>ProcessNextSIMbotEvent</c> processes the next SIMbot event in the queue.
    /// </summary>
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

    /// <summary>
    /// Method <c>RunPythonScript</c> sets up the Python environment and runs the user Python script.
    /// </summary>
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

    /// <summary>
    /// Method <c>HelloPython</c> logs a test string to the console.
    /// </summary>
    public void HelloPython()
    {
        Debug.Log("This method was called from python!");
    }

    /// <summary>
    /// Method <c>CreateMotor</c> is called from the Python script <c>sb</c> module. 
    /// </summary>
    /// <param name="id">Parameter <c>id</c> is the motor id.</param>
    /// <returns>a new <c>Motor</c> with the given <c>id</c></returns>
    public Motor CreateMotor(int id)
    {
        var newMotor = new Motor(id);
        motors.Add(newMotor);
        return newMotor;
    }

    /// <summary>
    /// Method <c>GenerateSpeedEvent</c> creates a new <c>SIMbotSpeedEvent</c> with the given <c>motor</c> and <c>speed</c> parameters.
    /// </summary>
    /// <param name="motor"><c>motor</c> is the given motor of the SIMbot.</param>
    /// <param name="speed"><c>speed</c> is the given speed of the motor.</param>
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

    /// <summary>
    /// Method <c>GenerateTimeSleepEvent</c> creates a new <c>SIMbotTimeSleepEvent</c> with the given <c>duration</c> parameter.
    /// </summary>
    /// <param name="duration"><c>duration</c> is the duration of time to sleep.</param>
    /// <returns>a new <c>SIMbotTimeSleepEvent</c></returns>
    public SIMbotEvent GenerateTimeSleepEvent(float duration)
    {
        var newEvent = new SIMbotTimeSleepEvent
        {
            Duration = duration
        };
        AddEventToEventList(newEvent);
        return newEvent;
    }

    /// <summary>
    /// Method <c>AddEventToEventList</c> adds the <c>newEvent</c> to the <c>_events</c> queue.
    /// </summary>
    /// <param name="newEvent"><c>newEvent</c> is the new event.</param>
    private void AddEventToEventList(SIMbotEvent newEvent)
    {
        _events.Add(newEvent);
    }
}

