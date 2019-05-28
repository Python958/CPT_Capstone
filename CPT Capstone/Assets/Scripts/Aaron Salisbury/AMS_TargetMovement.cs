using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AMS_TargetMovement : MonoBehaviour
{
    public GameObject loc1;
    public GameObject loc2;
    public float moveSpeed;
    private int moveTo = 1;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTo == 1)
        {
            agent.SetDestination(loc1.transform.position);
        }
        if (moveTo == 2)
        {
            agent.SetDestination(loc2.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == loc1)
        {
            Debug.Log("entered");
            moveTo = 2;
        }
        if (other.gameObject == loc2)
        {
            Debug.Log("second Enter");
            moveTo = 1;
        }
        Debug.Log("some collision");
    }
}
