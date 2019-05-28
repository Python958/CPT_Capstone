using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthExit : MonoBehaviour
{
    public bool playerIn;
    public bool spotted = false;
    public float seenTimer = 20f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        AMS_UniversalFunctions.GoToResultsScreen(true);
        
    }
}
