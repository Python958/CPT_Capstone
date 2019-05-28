using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ParentHive : MonoBehaviour
{
    //Place spawners that will deactivate when this hive dies in here
    public List<Transform> spawnLocations;
    public float spawnFrequency;

    public List<GameObject> enemyPool;

    [HideInInspector] public bool counting = true;
    private int spawnCount;
    private float currentWaveTime;

    //this is for capping the spawn rate
    public bool capSpawning = true;
    private List<AMS_Health_Management> enemiesSpawned;
    public int spawnCap;

    // Start is called before the first frame update
    void Start()
    {
        currentWaveTime = 0f;
        enemiesSpawned = new List<AMS_Health_Management>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            currentWaveTime += Time.deltaTime;
            if (currentWaveTime >= spawnFrequency)
            {
                currentWaveTime = 0f;
                if (AvailableSpawners())
                {
                    CleanHealthList(enemiesSpawned);
                    if (enemiesSpawned.Count < spawnCap)
                    {
                        SpawnEnemy();
                    }//only if we are below spawn cap
                }//only if we have active spawners
            }//spawn the bad guys
        }//toggle ability to count down on and off
    }

    private bool AvailableSpawners()
    {
        var tempBool = false;
        foreach (Transform spawnPoint in spawnLocations)
        {
            if (spawnPoint.gameObject.activeSelf) { tempBool = true; }
        }

        return (tempBool);
    }//just returns whether any of the spawners are even active;

    private Vector3 GetRandomPos()
    {
        var size = spawnLocations.Count;
        if (size > 0)
        {
            int escapeI = 0;
            int i = 0;
            bool useable = false;
            do
            {
                i = Random.Range(0, size);
                escapeI++;
                if (spawnLocations[i].gameObject.activeSelf)
                {
                    var cam = FindObjectOfType<Camera>();
                    if (cam != null)
                    {
                        //old way of looking (worked in editor but not in build)
                        //var pos = cam.WorldToViewportPoint(spawnLocations[i].position);
                        //if (pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1 && pos.z >= 0) { Debug.Log("that spawnpoint is visible"); }

                        var rend = spawnLocations[i].GetComponent<Renderer>();
                        if (rend != null)
                        {
                            if (rend.isVisible)
                            {
                                // Debug.Log("enemy spawn visible");
                            }
                            else { useable = true; }
                        }
                        //  else { Debug.Log("the wave spawn points each need a mesh renderer component to work even though they are invisible"); }
                    }
                    // else { Debug.Log("can't find camera"); }
                }
            }
            while (!useable && escapeI <= 200);

            if (escapeI > 200)
            {
                //Debug.Log("Can't find an active spawner");
                return (Vector3.zero);
            }
            else return (spawnLocations[i].position);
        }
        else
        {
            Debug.Log("this should not return empty vector3");
            return (new Vector3(0, 0, 0));
        }
    }//chooses random from location list

    private GameObject GetRandomEnemy()
    {
        var size = enemyPool.Count;
        if (size > 0)
        {
            var i = Random.Range(0, size);
            return (enemyPool[i]);
        }
        else { Debug.Log("no enemy pool set up or empty"); return (null); }
    }//returns random enemy from current pool

    private void SpawnEnemy()
    {
        var enemyType = GetRandomEnemy();
        var pos = GetRandomPos();
        if (pos == Vector3.zero)
        {
            //Debug.Log("failed to find spawn location for some reason");
        }
        else
        {
            DLC_EnemyManager eManage = new DLC_EnemyManager();
            eManage.e_Instance = Instantiate(enemyType, pos, transform.rotation);
            eManage.SetupAI(spawnLocations);

            spawnCount++;
            var health = eManage.e_Instance.GetComponentInChildren<AMS_Health_Management>();
            if (health != null) { enemiesSpawned.Add(health); }
            else { Debug.Log("we are spawning enemies with no health component!"); }
        }
    }//creates a random enemy at a random location and increments counter

    private void CleanHealthList(List<AMS_Health_Management> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] != null && !list[i].Equals(null))
            {
                if (list[i].currentHealth <= 0) { list.RemoveAt(i); }
            }
            else { list.RemoveAt(i); }
        }
    }
}
