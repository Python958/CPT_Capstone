using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_WinningItemSpawnScript : MonoBehaviour
{

    public GameObject spawnItem;
    public List<Transform> spawnPoints;
    //Never used so I commented this out
    //bool spawnWItem = false;
    int waveNum;
    bool once = false;
    //private GameObject waveSpawner;
    private WaveSpawner WS;
    // Start is called before the first frame update
    void Start()
    {
        WS = GameObject.FindObjectOfType<WaveSpawner>();
        once = true;
    }

    // Update is called once per frame
    void Update()
    {
        //resource now only spawns after the first wave. this way it does make the level a tad longer. (may bump it up to wave 3)
        waveNum = WS.currentWaveNumber;
        // Debug.Log(waveNum);
        if (waveNum >= 2 && once == true)
        {
            SpawnWItem();
            once = false;
        }
    }
    void SpawnWItem()
    {
        var size = spawnPoints.Count;
        //Debug.Log("I've spawned the resource");
        if (size > 0)
        {
            var index = Random.Range((int)0, size);
            var trans = spawnPoints[index];
            Instantiate(spawnItem, trans.position, trans.rotation);
        }
        else { Debug.Log("there aren't any found spawners"); }

        var spheres = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter sphere in spheres)
        {
            Destroy(sphere.gameObject);
        }
    }
}
