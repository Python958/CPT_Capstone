using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_DeployedHealthPack : MonoBehaviour
{
    //contrary to the name this does handle ammo boxes as well as anything else you want to drop and turn into another object

    public GameObject healthPack;
    public int healAmount;
    public string type;
    public float speed = 2;
    public float despawnTime = 15;
    public Material secondMat;

    // Start is called before the first frame update
    void Start()
    {
        if (type == "Unlock")
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                if (rend.materials[0].name.Contains("AmmoCrate1"))
                {
                    Debug.Log("should be changing mats");
                    rend.material = secondMat;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        GameObject tempItem = Instantiate(healthPack, transform.position, Quaternion.identity);
        var timer = tempItem.AddComponent<DespawnTimer>();
        timer.timerStart = despawnTime;

        //Used for ammo boxes
        if (type == "Unlock" || type == "Default")
        {
            tempItem.GetComponent<AMS_Ammo_Pickup>().type = type;
        }
        //used for health packs
        if (type == "Health")
        {
            tempItem.GetComponent<AMS_HealthPickup>().healAmount = healAmount;
        }
        Destroy(gameObject);
    }
}
