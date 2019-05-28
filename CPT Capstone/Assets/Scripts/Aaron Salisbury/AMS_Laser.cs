using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Laser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(1, transform.forward * 10000);
        }
    }
}
