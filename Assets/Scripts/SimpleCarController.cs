
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Wheel Collider Reference: https://docs.unity3d.com/Manual/class-WheelCollider.html
//WHeel Collider Tutorial Reference: https://docs.unity3d.com/Manual/WheelColliderTutorial.html

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public bool tankControls = true;

    private void Start()
    {
        //set the tank controls from the simbot script that has loaded the data previously
        tankControls = gameObject.GetComponent<SIMbot>().tankControls;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
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
}
