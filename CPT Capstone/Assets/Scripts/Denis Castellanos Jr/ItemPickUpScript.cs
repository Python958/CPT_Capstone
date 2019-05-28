using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpScript : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public float rotatingSpeed = 65;

    [Tooltip("Icons Placed here rotate along the Z axis at Time.deltaTime * rotating speed")]
    public GameObject GameObjectIcon;
    [Tooltip("Icons Placed here rotate along the Y axis at Time.deltaTime * rotating speed")]
    public GameObject GameObjectIcon2;
    [Tooltip("Icons Placed here rotate along the X axis at Time.deltaTime * rotating speed")]
    public GameObject GameObjectIcon3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameObjectIcon != null)
        {
            GameObjectIcon.transform.Rotate(new Vector3(x, y, Time.deltaTime * rotatingSpeed));
        }

        if (GameObjectIcon2 != null)
        {
            GameObjectIcon2.transform.Rotate(new Vector3(x, Time.deltaTime * rotatingSpeed, z));
        }

        if (GameObjectIcon3 != null)
        {
            GameObjectIcon3.transform.Rotate(new Vector3(Time.deltaTime * rotatingSpeed, y, z));
        }

    }
}
