
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private void Start()
    {
        //set the tank controls from the simbot script that has loaded the data previously
        tankControls = gameObject.GetComponent<SIMbot>().tankControls;
    }

    public void FixedUpdate()
    {
        //Speed of motors
        float speed = 2 * maxMotorTorque;

        //Break Speed
        float maxBreakTorque = speed;

        if (tankControls)
        {
            //Get Left Wheel Input W/S
            if (Input.GetKey("w"))
            {
                //Spins Forward
                axleInfos[0].leftWheel.motorTorque = speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
            }
            else if (Input.GetKey("s"))
            {
                //Spins Backward
                axleInfos[0].leftWheel.motorTorque = -speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].leftWheel.motorTorque = 0;
                axleInfos[0].leftWheel.brakeTorque = speed;
            }

            //Get Right Wheel Input I/K
            if (Input.GetKey("i"))
            {
                //Spins Forward
                axleInfos[0].rightWheel.motorTorque = speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (Input.GetKey("k"))
            {
                //Spins Backward
                axleInfos[0].rightWheel.motorTorque = -speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].rightWheel.motorTorque = 0;
                axleInfos[0].rightWheel.brakeTorque = speed;
            }

            //Arrow & WASD Controls
        }
        else
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            if (motor > 0)
            {
                //Front Wheels spin forward
                axleInfos[0].leftWheel.motorTorque = speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (motor < 0)
            {
                //Front Wheels spin backwards
                axleInfos[0].leftWheel.motorTorque = -speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = -speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else
            {
                //Stops Motor if no input
                axleInfos[0].leftWheel.motorTorque = 0;
                axleInfos[0].leftWheel.brakeTorque = speed;
                axleInfos[0].rightWheel.motorTorque = 0;
                axleInfos[0].rightWheel.brakeTorque = speed;
            }

            if (steering > 0)
            {
                //Turn Right
                axleInfos[0].leftWheel.motorTorque = speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = -speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
            else if (steering < 0)
            {
                //Turn Left
                axleInfos[0].leftWheel.motorTorque = -speed;
                axleInfos[0].leftWheel.brakeTorque = 0;
                axleInfos[0].rightWheel.motorTorque = speed;
                axleInfos[0].rightWheel.brakeTorque = 0;
            }
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
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
