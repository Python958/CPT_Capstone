using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_DeployedTauntTotem : MonoBehaviour
{
    [HideInInspector]
    public float healthOnDeploy;
    public float speed = 4;

    public GameObject tauntTotem;

    private bool once = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!once)
        {
            once = true;
            GameObject tempItem = Instantiate(tauntTotem, transform.position, Quaternion.identity);
            tempItem.GetComponentInChildren<AMS_Health_Management>().maxHealth = healthOnDeploy;
            tempItem.GetComponentInChildren<AMS_Health_Management>().currentHealth = healthOnDeploy;
            Destroy(gameObject);
        }
        Destroy(gameObject);
        
    }
}
