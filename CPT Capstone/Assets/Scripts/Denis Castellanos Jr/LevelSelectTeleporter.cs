using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectTeleporter : MonoBehaviour
{
    public GameObject Player;
    public Transform TeleportDestination;
    public bool TeleporterActivated;



    // Start is called before the first frame update
    void Start()
    {
        //Player = GameObject.Find("MainPlayer");
        Player = GameObject.FindGameObjectWithTag("Player");
        TeleporterActivated = false;
    }
    private void LateUpdate()
    {
        if (TeleporterActivated == true)
        {
            AMS_UniversalFunctions.GoToMissionBreifing(8, "Tutorial");
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleporterActivated = true;
            Debug.Log("The Player has activated the teleporter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleporterActivated = false;
            Debug.Log("The Player has left the teleporter");
        }
    }
}
