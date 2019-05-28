using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_AlternativeFireFragGrenadeControls : MonoBehaviour
{
    private Vector3 destination;
    public GameObject explosion;
    private bool movement = true;
    private float speed = 35f;
    private float timer;
    private float maxTime;
    public float minimumArmingTime;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
            var dist = Vector3.Distance(hit.point, transform.position);
            maxTime = Mathf.Max(dist / speed, minimumArmingTime);
            timer = 0f;
        }//changed this to set a timer based on the mouse distance from player and use that to detonate on target
    }

    // Update is called once per frame
    void Update()
    {
        if (movement)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        timer += Time.deltaTime;
        if (timer >= maxTime)
        {
            explosion.SetActive(true);
            movement = false;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Turret"
            && col.gameObject.tag != "Mine"
            && col.gameObject.tag != "Hazard"
            && col.gameObject.GetComponent<AMS_Bullet>() == null
            && col.gameObject.GetComponent<DLC_Checkpoint>() == null
            && col.gameObject.layer != 2)
        {
            if (timer >= minimumArmingTime)
            {
                movement = false;
                explosion.SetActive(true);
            }
            else { Destroy(gameObject); Debug.Log("Not Armed"); }
        }
    }
}