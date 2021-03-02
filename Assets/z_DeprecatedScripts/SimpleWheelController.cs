
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Wheel Collider Reference: https://docs.unity3d.com/Manual/class-WheelCollider.html
//WHeel Collider Tutorial Reference: https://docs.unity3d.com/Manual/WheelColliderTutorial.html

[System.Serializable]
public class WheelInfo
{
    public WheelCollider wheelCollider;
    //public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class SimpleWheelController : MonoBehaviour
{
    public WheelInfo axleInfo;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public int sps;
    //public bool tankControls = true;

    // finds the corresponding visual wheel
    // correctly applies the transform
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

    public void FixedUpdate()
    {
        //Speed of motors
        float speed = 2 * maxMotorTorque;
        int goalRPM = sps * 1000 * 60;

        if (axleInfo.wheelCollider.rpm < goalRPM)
        {
            axleInfo.wheelCollider.motorTorque += speed;
            axleInfo.wheelCollider.brakeTorque += 0;
        }
        else if (axleInfo.wheelCollider.rpm > goalRPM)
        {
            axleInfo.wheelCollider.motorTorque -= speed / 2;
            axleInfo.wheelCollider.brakeTorque += 0;
        }
        else
        {
            axleInfo.wheelCollider.motorTorque += 0;
            axleInfo.wheelCollider.brakeTorque += 0;
        }

        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        if (motor > 0)
        {
            //Front Wheels spin forward
            axleInfo.wheelCollider.motorTorque = speed;
            axleInfo.wheelCollider.brakeTorque = 0;
        }
        else if (motor < 0)
        {
            //Front Wheels spin backwards
            axleInfo.wheelCollider.motorTorque = -speed;
            axleInfo.wheelCollider.brakeTorque = 0;
        }
        else
        {
            //Stops Motor if no input
            axleInfo.wheelCollider.motorTorque = 0;
            axleInfo.wheelCollider.brakeTorque = speed;
        }

        if (steering > 0)
        {
            //Turn Right
            axleInfo.wheelCollider.motorTorque = speed;
            axleInfo.wheelCollider.brakeTorque = 0;
        }
        else if (steering < 0)
        {
            //Turn Left
            axleInfo.wheelCollider.motorTorque = -speed;
            axleInfo.wheelCollider.brakeTorque = 0;
        }

        ApplyLocalPositionToVisuals(axleInfo.wheelCollider);
        //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
    }
}
