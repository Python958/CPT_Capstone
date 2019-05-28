using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Aim : MonoBehaviour
{
    public float turnSpeed; //this won't be used unless you deal with below
    private Vector3 localOffset;

    private void Start()
    {
        var laserTrans = transform.Find("Laser Start");
        var bulletTrans = laserTrans.Find("BulletStart");
        localOffset = laserTrans.localPosition;// + bulletTrans.localPosition;
        localOffset = new Vector3(localOffset.x, 0f, 0f); //we only want the x offset
    }

    // Update is called once per frame
    void Update()
    {
        //make a plane to intersect the ray with instead of any object
        Plane yLevel = new Plane(Vector3.up, transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0.0f;

        if (yLevel.Raycast(mouseRay, out distance))
        {
            var lookPos = mouseRay.GetPoint(distance);
            var goalDirection = Quaternion.LookRotation(lookPos - transform.position);
            var tempOffset = (Quaternion.Euler(0f, 180f, 0f) * goalDirection) * localOffset;
            var newLookPos = lookPos + tempOffset;
            goalDirection = Quaternion.LookRotation(newLookPos - transform.position);
            
            transform.rotation = goalDirection;
            //Replace goalDirection in the assignment above with the code below if you want the aiming to the cursor to not be instantaneous.    
            //Quaternion.RotateTowards(transform.rotation, goalDirection, turnSpeed * Time.deltaTime);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var prim = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            prim.transform.position = transform.position + localOffset;
        }
        */
    }
}