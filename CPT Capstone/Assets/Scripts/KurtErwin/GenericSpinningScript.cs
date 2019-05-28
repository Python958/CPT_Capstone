using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpinningScript : MonoBehaviour
{

    public bool spinX;
    public bool spinY;
    public bool spinZ;
    public bool localValues = false;
    public float spinRate;
    
    // Update is called once per frame
    void Update()
    {
        var xRotation = spinX ? spinRate * Time.deltaTime : 0f;
        var yRotation = spinY ? spinRate * Time.deltaTime : 0f;
        var zRotation = spinZ ? spinRate * Time.deltaTime : 0f;

        var spinVec = new Vector3(xRotation, yRotation, zRotation);


        if (localValues){ transform.localRotation = transform.localRotation * Quaternion.Euler(spinVec); }

        if (!localValues) { transform.Rotate(spinVec, Space.World); }
    }
}
