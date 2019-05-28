using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_Hostility_Bar : MonoBehaviour
{
    public AMS_SpawnRateController SpawnRateController;
    public GameObject[] hostilityMeter;
    private float remainder;
    private float startingSpawn;
    private float lowestSpawn;
    

    // Start is called before the first frame update
    void Start()
    {

        SpawnRateController = FindObjectOfType<AMS_SpawnRateController>();

        startingSpawn = SpawnRateController.defaultEnemySpawnRate; // 8 seconds
        lowestSpawn = startingSpawn * 2;  // 8 * 2 = 16 seconds
    }

    // Update is called once per frame
    void Update()
    {
        remainder = SpawnRateController.defaultEnemySpawnRate / lowestSpawn; 
        
        if(remainder >= 0.1f)
        {
            hostilityMeter[8].SetActive(false);
        }
        else
        {
            hostilityMeter[8].SetActive(true);
        }
        if (remainder >= 0.2f)
        {
            hostilityMeter[7].SetActive(false);
        }
        else
        {
            hostilityMeter[7].SetActive(true);
        }
        if (remainder >= 0.3f)
        {
            hostilityMeter[6].SetActive(false);
        }
        else
        {
            hostilityMeter[6].SetActive(true);
        }
        if (remainder >= 0.4f)
        {
            hostilityMeter[5].SetActive(false);
        }
        else
        {
            hostilityMeter[5].SetActive(true);
        }
        if (remainder >= 0.5f)
        {
            hostilityMeter[4].SetActive(false);
        }
        else
        {
            hostilityMeter[4].SetActive(true);
        }
        if (remainder >= 0.6f)
        {
            hostilityMeter[3].SetActive(false);
        }
        else
        {
            hostilityMeter[3].SetActive(true);
        }
        if (remainder >= 0.7f)
        {
            hostilityMeter[2].SetActive(false);
        }
        else
        {
            hostilityMeter[2].SetActive(true);
        }
        if (remainder >= 0.8f)
        {
            hostilityMeter[1].SetActive(false);
        }
        else
        {
            hostilityMeter[1].SetActive(true);
        }
        if (remainder >= 0.9f)
        {
            hostilityMeter[0].SetActive(false);
        }
        else
        {
            hostilityMeter[0].SetActive(true);
        }

    }
}
