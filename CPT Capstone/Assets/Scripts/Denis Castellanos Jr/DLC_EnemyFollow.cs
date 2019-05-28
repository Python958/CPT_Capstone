using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_EnemyFollow : MonoBehaviour
{
    public float speed;
    private Transform target;
    private bool enemyIsAlive = true;
    public AMS_Health_Management _Health_Management;

    // Use this for initialization

    private void Awake()
    {
        _Health_Management = GetComponent<AMS_Health_Management>();
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyIsAlive)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }

            if (_Health_Management.currentHealth <= 0)
            {
                enemyIsAlive = false;
            }

        }
       

    }
    
}
