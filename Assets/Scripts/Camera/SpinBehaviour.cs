using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBehaviour : MonoBehaviour
{
    public float spinSpeed = 2;

    void Update()
    {
        transform.Rotate (0, spinSpeed * Time.deltaTime, 0);
    }
}
