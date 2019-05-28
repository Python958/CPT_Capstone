using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{

    public GameObject[] TeleportersTag;
    public Teleporter[] Teleporters;
   
    

    // Start is called before the first frame update
    void Start()
    {
        TeleportersTag = GameObject.FindGameObjectsWithTag("Teleporter");
        TeleporterStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TeleporterStats()
    {
        for (int i = 0; i < Teleporters.Length; i++)
        {
            Teleporters[i] = GetComponentInChildren<Teleporter>();
        }
    }
    
}
