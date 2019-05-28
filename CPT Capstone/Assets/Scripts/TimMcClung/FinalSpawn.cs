using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class FinalSpawn : MonoBehaviour
{
    public bool inTrigger = false;
    public bool isSpawned = false;
    public GameObject boss;
    public List<Transform> spawnLocations;
    private GameObject note;
    private bool canvasShow = false;
    public AudioClip bossSpawnSound;

    public int bossCounter = 0;

    void Start()
    {
        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if (note == null) { Debug.Log("can't find canvas gameObject"); }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTrigger == true)
        {
            SpawnBoss();
            bossCounter++;
            isSpawned = true;
        }
        if (canvasShow == true)
        {
            note.SetActive(true);
        }
        if (canvasShow == false)
        {
            note.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        canvasShow = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
        canvasShow = false;
    }

    void SpawnBoss()
    {
        var enemyType = boss;
        var pos = GetBossPos();
        if (bossCounter < 1)
        {
            DLC_EnemyManager eManage = new DLC_EnemyManager();
            eManage.e_Instance = Instantiate(enemyType, pos, transform.rotation);
            eManage.SetupAI(spawnLocations);

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }

    private Vector3 GetBossPos()
    {
        var size = spawnLocations.Count;
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


                    var rend = spawnLocations[i].GetComponent<Renderer>();
                    if (rend != null)
                    {
                        if (rend.isVisible) { Debug.Log("enemy spawn visible"); }
                        else { useable = true; }
                    }
                    else { Debug.Log("the wave spawn points each need a mesh renderer component to work even though they are invisible"); }
                }
                else { Debug.Log("can't find camera"); }
            }
        }
        while (!useable && escapeI <= 200);

        if (escapeI > 200) { Debug.Log("Can't find an active spawner"); return (transform.position); }
        else return (spawnLocations[i].position); 
    }
}

