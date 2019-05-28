using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_ThrownMine : MonoBehaviour
{
    public float dist;
    public float maxDist;
    public Vector3 destination;
    private Vector3 startLocation;
    public GameObject mine;
    public float speed = 4;
    public GameObject player; 
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
        player = FindObjectOfType<AMS_OffHand_Weapon>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //Went to far
        if (Vector3.Distance(new Vector3 (startLocation.x, 0, startLocation.z), new Vector3 (transform.position.x, 0, transform.position.z))>= maxDist)
        {
            Instantiate(mine, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //If it arrives
        if (transform.position == destination)
        {
            Instantiate(mine, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        while(!Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z)))
            {
            transform.Translate(Vector3.down * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            GameObject tempMine = Instantiate(mine,transform.position, Quaternion.identity);
            tempMine.transform.LookAt(player.transform);
            tempMine.transform.localEulerAngles = new Vector3(tempMine.transform.localEulerAngles.x + 90, tempMine.transform.localEulerAngles.y, tempMine.transform.localEulerAngles.z);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Ramp")
        {
            Instantiate(mine, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
