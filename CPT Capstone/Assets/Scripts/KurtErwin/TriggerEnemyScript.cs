using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class TriggerEnemyScript : MonoBehaviour
{
    public int enemyCount;                      //how many enemy this spawner should spawn

    public List<Transform> spawnLocations;      //pool of locations to spawn at
    public List<GameObject> enemyPool;          //this is the pool of enemy to draw from for the spawning
    public bool hasTriggered = false;
    public bool allowedToTrigger = true;
    private DLC_CheckPointController checkPointController;
    public GameObject myCheckPoint;

    private void Start()
    {
        checkPointController = GameObject.FindObjectOfType<DLC_CheckPointController>();
        if(checkPointController == null) { Debug.Log("can't find checkpoint controller component"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && allowedToTrigger && other.gameObject.tag == "Player")
        {
            if (myCheckPoint == null || checkPointController.IsCheckPointActive(myCheckPoint))
            {
                hasTriggered = true;
                var count = 0;
                while (count < enemyCount)
                {
                    SpawnEnemy();
                    count++;
                }
            }
            else { Debug.Log("myCheckPoint is null or player has not activated the correct check point right now"); }
        }
    }

    private void SpawnEnemy()
    {
        var enemyType = GetRandomEnemy();
        var pos = GetRandomPos();

        DLC_EnemyManager  eManage = new DLC_EnemyManager();
        eManage.e_Instance = Instantiate(enemyType, pos, transform.rotation);
        eManage.SetupAI(spawnLocations);
    }

    private Vector3 GetRandomPos()
    {
        var size = spawnLocations.Count;
        var i = Random.Range(0, size);
        return (spawnLocations[i].position);
    }

    private GameObject GetRandomEnemy()
    {
        var size = enemyPool.Count;
        var i = Random.Range(0, size);
        return (enemyPool[i]);
    }
}
