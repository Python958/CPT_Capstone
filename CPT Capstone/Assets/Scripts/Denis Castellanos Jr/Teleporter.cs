using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
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
           Player.transform.position = TeleportDestination.position;
            // Player.transform.position = Vector3.Lerp(Player.transform.position, TeleportDestination.transform.position, 1f);
          //  Player.transform.localPosition = TeleportDestination.localPosition;
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
