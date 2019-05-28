using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public GameObject MainPlayer;
    public GameObject MiniMapCamera;
    public GameObject Base;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MainPlayer = GameObject.Find("MainPlayer");
        MiniMapCamera = MainPlayer.transform.Find("MiniMap_Camera").gameObject;
        Base = GameObject.FindGameObjectWithTag("Base");
        if(Base = null) { Debug.Log("Base is not in the scene"); }
    }

    void LateUpdate()
     {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y; //sets the Y Position of the Camera
        transform.position = newPosition;
        MiniMapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        
      }

}
