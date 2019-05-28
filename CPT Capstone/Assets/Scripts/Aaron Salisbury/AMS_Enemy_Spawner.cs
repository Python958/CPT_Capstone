using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Enemy_Spawner : MonoBehaviour
{

    public GameObject spawnedEnemy;
    private AMS_SpawnRateController controller;
    private float currentSpawnrate;
    public bool activeWave;
    //private int currentWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        activeWave = true;
        controller = GameObject.FindObjectOfType<AMS_SpawnRateController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if wave is on
        if (controller.CanSpawn() && activeWave)
        {
            currentSpawnrate += Time.deltaTime * 1;
            if (currentSpawnrate >= controller.defaultEnemySpawnRate)
            {
                if (Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity, 0, QueryTriggerInteraction.Ignore).Length == 0)
                {
                    Instantiate(spawnedEnemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    currentSpawnrate = 0;
                }
            }
        }
    }
}
