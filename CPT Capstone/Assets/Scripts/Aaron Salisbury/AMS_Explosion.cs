﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Explosion : MonoBehaviour
{
    public int damage = 50;
    public float duration = 1;
    public GameObject creator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            Destroy(creator);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<AMS_Health_Management>() == true)
        {
            collision.gameObject.GetComponent<AMS_Health_Management>().TakeDamage(50, "Explosion");
        }
    }
}
