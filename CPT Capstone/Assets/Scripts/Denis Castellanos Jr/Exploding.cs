using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploding : MonoBehaviour
{
    public GameObject Blast;
    public bool DidTheEnemyHit; //Checks if the Enemy has it another object
    private float DetonationCoolDown; //The Cooldown before exploding again
    // Start is called before the first frame update
    void Start()
    {
        DidTheEnemyHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DidTheEnemyHit == true)
        {
            //Debug.Log("The Enemy Hit and is now going to explode");
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //&& other.gameObject.GetComponent<AMS_Health_Management>() == true)
        {
            DidTheEnemyHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<AMS_Health_Management>() == true)
        {
            DidTheEnemyHit = false;
            Debug.Log("The Player ran away");
        }
    }

    private void Explode()
    {
        var boom = Instantiate(Blast, transform.position, transform.rotation);

        var final = transform;
        while (final.parent != null) final = final.parent;
        Destroy(final.gameObject);
    }
}