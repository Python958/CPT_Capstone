using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningItemSpawnerScript : MonoBehaviour
{
    public GameObject WinningItemObject;
    public WinningItem WinningItemScript;

    public float chanceToSpawn;

    [Header("V Do not assign in inspector V")]
    public GameObject ownedResource;
    private bool stillSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        WinningItemScript = WinningItemObject.GetComponent<WinningItem>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (stillSpawning) { SpawnResource(); }

        if (ownedResource != null)
        {
            var dis = Vector3.Distance(gameObject.transform.position, ownedResource.transform.position);

            if (dis > 2) { ownedResource = null; }
        }
    
    }

    public void SpawnResource()
    {
        WinningItem[] resourcesPresent = GameObject.FindObjectsOfType<WinningItem>();

        if (resourcesPresent.Length <= 1 && ownedResource == null)
        {
            if (Random.Range(10, 100) < chanceToSpawn)
            {
                ownedResource = Instantiate(WinningItemObject, gameObject.transform.position, gameObject.transform.rotation);
               /* var resScript = ownedResource.GetComponent<WinningItem>();
                if (resScript != null)
                {
                    resScript.respawn = true;
                }
                else { Debug.Log("oops, resource doesn't have resource script component"); }
            */
    }
        }
        else
        {
            stillSpawning = false;
        }
    }
}
