
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

//Wheel Collider Reference: https://docs.unity3d.com/Manual/class-WheelCollider.html
//WHeel Collider Tutorial Reference: https://docs.unity3d.com/Manual/WheelColliderTutorial.html

[System.Serializable]

/// <summary>Class <c>AxleInfo</c> holds a set of wheels, either the front wheels or the back wheels.</summary>
public class AxleInfo {
    /// <summary>Field <c>leftWheel</c> is the left wheel of the set.</summary>
    public WheelCollider leftWheel;
    /// <summary>Field <c>rightWheel</c> is the right wheel of the set.</summary>
    public WheelCollider rightWheel;
}


/// <summary>Class <c>SimpleCarController</c> controls which wheels are moving based on the user input, how the wheels are graphically changing, and how fast the wheels are rotating.</summary>
public class SimpleCarController : MonoBehaviour {
    /// <summary>Property <c>axleInfos</c> is a list of wheel pairs. The entire list consists of the pair of front wheels, and the pair of back wheels. This was written by the group before ours and could be cleaned up.</summary>
    public List<AxleInfo> axleInfos;
    /// <summary>Property <c>maxMotorTorque</c> is how fast the wheels turn. In other words, how fast the SIMbot is moving.</summary>
    public float maxMotorTorque;
    /// <summary>Property <c>maxSteeringAngle</c> is whether the SIMbot is turning left or right.</summary>
    public float maxSteeringAngle;
    /// <summary>Property <c>tankControls</c> is which SIMbot controls are being used.</summary>
    public bool tankControls = true;

    /// <summary>Field <c>PythonControls</c> determines whether to use Python to drive.</summary>
    private bool _pythonControls = false;

    private PythonBot _pythonBotScript;

    private Rigidbody rb;

    private int _maxRpm = 480;

    private void Awake()
    {
        //get the rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        //get the PythonBot script
        _pythonBotScript = gameObject.GetComponent<PythonBot>();

        //set the tank controls from the simbot script that has loaded the data previously
        tankControls = gameObject.GetComponent<SIMbot>().tankControls;

        //set whether to use python or not
        _pythonControls = gameObject.GetComponent<SIMbot>().pythonBot;
    }

    private void Start()
    {
        if (_pythonControls)
        {
            _pythonBotScript.OnSpeedChange += HandleSpeedChange;
        }
    }

    /// <summary>
    /// Listens for OnSpeedChange event and changes the motor speed accordingly.
    /// </summary>
    /// <param name="id"><c>id</c> is the id of the motor</param>
    /// <param name="speed"><c>speed</c> is the speed to set the motor to</param>
    private void HandleSpeedChange(int id, float speed)
    {
        if (id == 1)
        {
                axleInfos[0].leftWheel.motorTorque = speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
        }
        else
        {
                axleInfos[0].rightWheel.motorTorque = speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
        }
    }

    public void FixedUpdate()
    {
        //Speed of motors
        float speed = 2 * maxMotorTorque;

        //Break Speed
        float maxBreakTorque = speed;


        var currentSpeed = rb.velocity.magnitude;

        var leftWheel = axleInfos[0].leftWheel;
        var rightWheel = axleInfos[0].rightWheel;
        //If using python controls, look for python events, ignore input
        if (_pythonControls)
        {
            //clamp RPM for python
            ClampMotorRpm(rightWheel, 2);
            ClampMotorRpm(leftWheel, 1);
        }
        //user input controls
        else
        {
            //tank controls
            if (tankControls)
            {
                HandleTankControlInput(speed);
                
            }
            else //Arrow & WASD Controls
            {
                HandleWasdInput(speed);
            }

            //clamp RPM
            if (Mathf.Abs(rightWheel.rpm) > _maxRpm)
            {
                rightWheel.motorTorque = 0;
            }

            if (Mathf.Abs(leftWheel.rpm) > _maxRpm)
            {
                leftWheel.motorTorque = 0;
            }
        }
        
        //update the visuals
        foreach (AxleInfo axleInfo in axleInfos)
        {
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        void ClampMotorRpm(WheelCollider wC, int motorId)
        {
            if (wC.rpm > _maxRpm)
            {
                wC.motorTorque = 0;
            }
            else
            {
                wC.motorTorque = _pythonBotScript.GetMotor(motorId).speed();
            }
        }

        void HandleTankControlInput(float f)
        {
            //Get Left Wheel Input W/S
            if (Input.GetKey("w"))
            {
                //Spins Forward
                axleInfos[0].leftWheel.motorTorque = f;
                axleInfos[0].leftWheel.brakeTorque = 0;
            }
            else if (Input.GetKey("s"))
            {
                //Spins Backward
                axleInfos[0].leftWheel.motorTorque = -f;
                axleInfos[0].leftWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].leftWheel.motorTorque = 0;
                axleInfos[0].leftWheel.brakeTorque = f;
            }

            //Get Right Wheel Input I/K
            if (Input.GetKey("i"))
            {
                //Spins Forward
                axleInfos[0].rightWheel.motorTorque = f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (Input.GetKey("k"))
            {
                //Spins Backward
                axleInfos[0].rightWheel.motorTorque = -f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].rightWheel.motorTorque = 0;
                axleInfos[0].rightWheel.brakeTorque = f;
            }
        }

        void HandleWasdInput(float f)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            if (motor > 0)
            {
                //Front Wheels spin forward
                axleInfos[0].leftWheel.motorTorque = f;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (motor < 0)
            {
                //Front Wheels spin backwards
                axleInfos[0].leftWheel.motorTorque = -f;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = -f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].leftWheel.motorTorque = 0;
                axleInfos[0].leftWheel.brakeTorque = f;
                axleInfos[0].rightWheel.motorTorque = 0;
                axleInfos[0].rightWheel.brakeTorque = f;
            }

            if (steering > 0)
            {
                //Turn Right
                axleInfos[0].leftWheel.motorTorque = f;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = -f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (steering < 0)
            {
                //Turn Left
                axleInfos[0].leftWheel.motorTorque = -f;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = f;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
        }
    }

    /// <summary>Method <c>ApplyLocalPositionToVisuals</c> finds the corresponding visual wheel and correctly applies the transform to it.</summary>
    /// <param><c>collider</c> is the wheel collider to apply the transform to.</param>
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
