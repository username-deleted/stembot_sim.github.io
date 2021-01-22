
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

        //Finds each right and left wheel with motors
        foreach (AxleInfo axleInfo in axleInfos) {

            if (tankControls) {
                //Get Left Wheel Input W/S
                if (Input.GetKey("w")) {
                    //Spins Forward
                    axleInfo.leftWheel.motorTorque = speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                } else if (Input.GetKey("s")) {
                    //Spins Backward
                    axleInfo.leftWheel.motorTorque = -speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                } else {
                    //Stops Motor if no input
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = speed;
                }

                //Get Right Wheel Input I/K
                if (Input.GetKey("i")) {
                    //Spins Forward
                    axleInfo.rightWheel.motorTorque = speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                } else if (Input.GetKey("k")) {
                    //Spins Backward
                    axleInfo.rightWheel.motorTorque = -speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                } else {
                    //Stops Motor if no input
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.rightWheel.brakeTorque = speed;
                }

            //Arrow & WASD Controls
            } else {
                float motor = maxMotorTorque * Input.GetAxis("Vertical");
                float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
                if (motor > 0) {
                    //Front Wheels spin forward
                    axleInfo.leftWheel.motorTorque = speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.motorTorque = speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                } else if (motor < 0) {
                    //Front Wheels spin backwards
                    axleInfo.leftWheel.motorTorque = -speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.motorTorque = -speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                } else {
                    //Stops Motor if no input
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = speed;
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.rightWheel.brakeTorque = speed;
                }

                if (steering > 0) {
                    //Turn Right
                    axleInfo.leftWheel.motorTorque = speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.motorTorque = -speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                } else if (steering < 0) {
                    //Turn Left
                    axleInfo.leftWheel.motorTorque = -speed;
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.motorTorque = speed;
                    axleInfo.rightWheel.brakeTorque = 0;
                }

            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}
