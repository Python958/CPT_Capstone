using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnerScript : MonoBehaviour
{
    public GameObject spawnBase;
    public List<Transform> baseSpawnPoints;
    public WaveSpawner waveSpawnerScript;
    public int currentWaveNumber;
    public int startingWaveNumber;
    public bool WasThereAWaveChange;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawnerScript = FindObjectOfType<WaveSpawner>();
      //  spawnBase = GameObject.Find("Base");
        startingWaveNumber = currentWaveNumber;
        currentWaveNumber = waveSpawnerScript.currentWaveNumber;
        BaseSpawner();
        WasThereAWaveChange = waveSpawnerScript.WasThereAWaveChange;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("The startingWave Number is = " + startingWaveNumber);
        //  Debug.Log("The currentWave Number is = " + currentWaveNumber);
        if (WasThereAWaveChange)
        {
            ChangeBaseLocation();
        }
        else
        {
            WasThereAWaveChange = false;
        }
        
        
    }

    void ChangeBaseLocation()
    {
        if (WasThereAWaveChange == true)
        {
            Destroy(GameObject.Find("Base(Clone)"));
            var size = baseSpawnPoints.Count;
            if (size > 0)
            {
                var index = Random.Range((int)0, size);
                var trans = baseSpawnPoints[index];
               // spawnBase.transform.position = trans.position;
                Instantiate(spawnBase, trans.position, trans.rotation);
                {
                    WasThereAWaveChange = false;
                }
                Debug.Log("The Base was spawned");
            }
        }
        else
        {
            WasThereAWaveChange = false;
        }
    }

    void BaseSpawner()
    {
        var size = baseSpawnPoints.Count;
        if (size > 0)
        {
            var index = Random.Range((int)0, size);
            var trans = baseSpawnPoints[index];
            Instantiate(spawnBase, trans.position, trans.rotation);
            Debug.Log("The Base was spawned");
        }
        else
        {
            Debug.Log("there aren't any base spawners");
        }

        var spheres = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter sphere in spheres)
        {
            
            //Destroy(sphere.gameObject);
        }
    }
}
