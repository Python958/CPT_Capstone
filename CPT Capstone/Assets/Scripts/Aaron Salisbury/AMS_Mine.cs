using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Mine : MonoBehaviour
{
    public float armTime;
    private bool armed = false;
    public GameObject explosion;
    public GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!armed)
        {
            if (armTime >= 0)
            {
                armTime = armTime - Time.deltaTime;
            }
            if (armTime <= 0)
            {
                armed = true;
            }
        }
        if (armed && laser.activeSelf == false)
        {
            laser.SetActive(true);
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        //GameObjects that cause the explosion
        if (collision.gameObject.tag == "Default_Enemy" || collision.gameObject.tag == "Target")
        {
            if(armed)
            {
                TriggerExplosion();
            }
        }
    }

    private void TriggerExplosion()
    {
        laser.SetActive(false);
        explosion.SetActive(true);
    }
}
