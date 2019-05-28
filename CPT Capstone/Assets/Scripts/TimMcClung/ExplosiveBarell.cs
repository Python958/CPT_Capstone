using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarell : MonoBehaviour
{
    float curhealth;
    public GameObject fireBomb;
    private GameObject barrel;
    
    // Start is called before the first frame update
    void Start()
    {
        //explodeWith.SetActive(false);
        barrel = GetComponentInChildren<AMS_Health_Management>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {  
        curhealth = barrel.GetComponent<AMS_Health_Management>().currentHealth;
        if (curhealth <= 0f)
        {
            Instantiate(fireBomb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
