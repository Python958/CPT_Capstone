using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var euler = transform.eulerAngles;
        euler.y = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = euler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
