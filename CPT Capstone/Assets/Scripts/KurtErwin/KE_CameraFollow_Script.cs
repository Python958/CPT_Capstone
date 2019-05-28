using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KE_CameraFollow_Script : MonoBehaviour
{
    //this script could be much improved if we moved the walls to a separate layer so that those are the only things being checked.
    //otherwise it is likely at some point the camera will try to avoid looking through things like other actors or miscellaneous stuff.

    public GameObject focusObject;              //whatever we want the camera to focus on

    public float height;                        //how far up the camera is positioned
    public float goalOffset;                    //this is how far down from the player position the camera wants to be
    public float angleChangeSpeed;              //this is how fast the camera is allowed to change angles
    public float zoomSpeed;                     //how fast the camera zooms into player
    private float currentOffset;                //this is how far down from the player position the camera is
    private float currentHeight;                //this is how close the camera currently is to the player
    public float playerColliderOffset;          //this is so the line does not intersect the player for collisions

    void Start()
    {
        currentOffset = goalOffset;
        currentHeight = height;
    }

    void LateUpdate()
    {
        if (focusObject != null)
        {

            //go ahead and move camera to it's new position
            transform.position = focusObject.transform.position + new Vector3(0, currentHeight, currentOffset);
            transform.LookAt(focusObject.transform);

            //check to see if you can't see the player
            var rayDist = Vector3.Distance(transform.position, focusObject.transform.position);
            var hitWall = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), rayDist - playerColliderOffset, 1, QueryTriggerInteraction.Ignore);

            //move it closer to vertical if you can't see the player
            if (hitWall)
            {
                if (currentOffset != 0)
                {
                    currentOffset = Mathf.Min(currentOffset + angleChangeSpeed, 0);
                }
                else if (currentHeight != 0)
                {
                    currentHeight = Mathf.Max(currentHeight - zoomSpeed, 0);
                }
            }


            //if the camera is not at goal try to move it back
            else
            {
                if(currentHeight != height)
                {
                    var checkPos = new Vector3(transform.position.x, transform.position.y+zoomSpeed, transform.position.z);
                    hitWall = Physics.Raycast(checkPos, transform.TransformDirection(Vector3.forward), rayDist - playerColliderOffset);

                    if (!hitWall) currentHeight = Mathf.Max(currentOffset + zoomSpeed, height);
                }
                else if (currentOffset != goalOffset)
                {
                    var checkPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - angleChangeSpeed);
                    hitWall = Physics.Raycast(checkPos, transform.TransformDirection(Vector3.forward), rayDist - playerColliderOffset);

                    if (!hitWall) currentOffset = Mathf.Max(currentOffset - angleChangeSpeed, goalOffset);
                }
            }
        }
        else
        {
            Debug.Log("can't find focus object");
        }
    }
}
