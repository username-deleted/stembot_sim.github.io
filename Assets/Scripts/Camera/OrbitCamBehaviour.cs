using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamBehaviour : MonoBehaviour {
    public float camSensitivity = 4.0f;
    public GameObject pivotPoint;

    private void Update() {
        float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        Vector3 movementVector = new Vector3(0, mouseX, 0);
        pivotPoint.transform.Rotate(movementVector * camSensitivity);
    }

  }
