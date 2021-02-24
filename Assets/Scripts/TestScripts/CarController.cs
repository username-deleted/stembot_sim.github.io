using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    private WheelCollider frontRightW, frontLeftW, backRightW, backLeftW;
    private Transform frontRightT, frontLeftT, backRightT, backLeftT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    private void Start()
    {
        //This allows us to set the variables in code rather than through the editor with the use of tags.
        //It makes it easier for new users to move things around between scenes without having to plug everything back in on the editor.
        frontRightT = GameObject.FindGameObjectWithTag("FrontRightT").GetComponent<Transform>();
        frontRightW = GameObject.FindGameObjectWithTag("FrontRightW").GetComponent<WheelCollider>();
        frontLeftT = GameObject.FindGameObjectWithTag("FrontLeftT").GetComponent<Transform>();
        frontLeftW = GameObject.FindGameObjectWithTag("FrontLeftW").GetComponent<WheelCollider>();
        backRightT = GameObject.FindGameObjectWithTag("BackRightT").GetComponent<Transform>();
        backRightW = GameObject.FindGameObjectWithTag("BackRightW").GetComponent<WheelCollider>();
        backLeftT = GameObject.FindGameObjectWithTag("BackLeftT").GetComponent<Transform>();
        backLeftW = GameObject.FindGameObjectWithTag("BackLeftW").GetComponent<WheelCollider>();
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    public void GetInput() {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer() {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontRightW.steerAngle = m_steeringAngle;
        frontLeftW.steerAngle = m_steeringAngle;
    }

    //STIMbots are front wheel drive, so we only accelerate the front wheels.
    private void Accelerate() {
        frontLeftW.motorTorque = m_verticalInput * motorForce;
        frontRightW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses() {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
    }

    //Watched a tutorial: https://www.youtube.com/watch?v=j6_SMdWeGFI
    //Go to 20:30 in the video to learn what "collider.GetWorldPose(out position, out quaternion);" does.
    private void UpdateWheelPose(WheelCollider collider, Transform transform) {
        Vector3 position = transform.position;
        Quaternion quaternion = transform.rotation;

        collider.GetWorldPose(out position, out quaternion);

        transform.position = position;
        transform.rotation = quaternion;
    }
}
