using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool destroy = false;
    public int scoreGainedOnDeposit = 0;
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
        if (other.gameObject.tag == "Door")
        {
            AMS_ScoreController.increaseScore(scoreGainedOnDeposit);

            Destroy(gameObject);
            //increment resource pool in here some where
        }
    }
}
