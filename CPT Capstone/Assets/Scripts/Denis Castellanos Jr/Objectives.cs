using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objectives : MonoBehaviour
{

    public GameObject StartObjective;
    public GameObject CompleteObjective;
    public GameObject Player;
   

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Objective Started");
        }
    }
}
