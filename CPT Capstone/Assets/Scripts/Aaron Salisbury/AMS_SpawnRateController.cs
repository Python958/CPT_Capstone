using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_SpawnRateController : MonoBehaviour
{

    public float defaultEnemySpawnRate;
    public float maxdefaultEnemySpawnrate;
    public float changeRate;
    private bool allowSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (maxdefaultEnemySpawnrate < defaultEnemySpawnRate)
        {
            defaultEnemySpawnRate -= changeRate * Time.deltaTime;
        }
    }

    public void ToggleSpawning()
    {
        allowSpawning = !allowSpawning;
    }
    public bool CanSpawn()
    {
        return (allowSpawning);
    }
}
