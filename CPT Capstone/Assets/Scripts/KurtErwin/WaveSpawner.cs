using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public List<Transform> spawnLocations;
    [HideInInspector] public SpawnWave[] waves;

    private SpawnWave currentWave;
    public int currentWaveNumber;
    [HideInInspector] public float currentWaveTime;

    [HideInInspector] public float coolDownTimer;
    private float currentWaveMaxTime;

    [HideInInspector]public bool counting = true;
    private int spawnCount; 
    private int spawnMaxCount;
    public bool repeatWaves = true;
    
    private bool careAboutEnemies = true;
    [HideInInspector]
    public List<AMS_Health_Management> enemiesSpawned; //made public for gui

    public bool trappedWave = false;
    public bool fireWave = false;
    private GameObject fireFloor;
    private GameObject trapFloor;

    public GameObject arrowObj;
    public bool WasThereAWaveChange = false;

    //Win by finishing the final wave
    [Tooltip("Off nothing happens upon reaching the final wave. On the player wins when reachign the final wave.")]
    public bool winningWave = false;
    public bool WinningWaveWasBeaten = false;

    public AudioClip waveStartSound;
    public AudioClip coolDownStartSound;
    private bool hasPlayedCoolDownSound = false;

    // Start is called before the first frame update
    void Start()
    {
        //!!!this looks like it will just get one of the random waves... is this what you want? If you want the variable from the current wave use that instead of get component. Talk to me if you need help -KE
        fireWave = GetComponent<SpawnWave>().waveOfFire;
        trappedWave = GetComponent<SpawnWave>().trapedFloor;
        fireFloor = GetComponent<SpawnWave>().fireFloor;
        trapFloor = GetComponent<SpawnWave>().trapFloor;
        currentWaveNumber = 0;
        currentWave = null;
        waves = GetComponents<SpawnWave>();

        enemiesSpawned = new List<AMS_Health_Management>();
        SetUpWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (trappedWave == true)
        {
            if (trapFloor != null) { trapFloor.SetActive(true); }
        }
        if (trappedWave == false)
        {
            if (trapFloor != null) { trapFloor.SetActive(false); }
        }
        if (fireWave == true) // looks for bool set in the inspector of whether or not to enable the fire floor (hazard must be in scene first)
        {
            if (fireFloor != null) { fireFloor.SetActive(true); }
        }
        if (fireWave == false)
        {
            if (fireFloor != null) { fireFloor.SetActive(false); }
        }

        if (counting)
        {
            if (currentWaveTime > 0f)
            {
                currentWaveTime -= Time.deltaTime;
                var numberShouldHaveSpawned = Mathf.FloorToInt(((currentWaveMaxTime - currentWaveTime)/currentWaveMaxTime)*spawnMaxCount);
                if(numberShouldHaveSpawned > spawnCount)
                {
                    SpawnEnemy();
                }//we have not spawned as many as we should have by this time
            }//during an active wave

            else if (currentWaveTime > -coolDownTimer)
            {
                if (!hasPlayedCoolDownSound)
                {
                    PlaySound(coolDownStartSound);
                    hasPlayedCoolDownSound = true;
                }
                if (spawnMaxCount > spawnCount)
                {
                    SpawnEnemy();
                }//Finish spawning remaining enemies
                if (careAboutEnemies)
                {
                    if (enemiesSpawned.Count == 0)
                    {
                        currentWaveTime -= Time.deltaTime;
                    }//no more enemies from previous waves
                    else
                    {
                        currentWaveTime = 0f;
                        CleanHealthList(enemiesSpawned);
                    }
                }//we do care about how many enemies killed from previous waves
                else
                {
                    currentWaveTime -= Time.deltaTime;
                }
            }//in the cool down

            else if (currentWaveTime <= -coolDownTimer)
            {
                //Debug.Log("new wave incoming");
                SetUpWave();
            }//cool down has ended and time to start spawning again
        }//toggle ability to count down on and off
    }

    private SpawnWave FindNextWave()
    {
        var returnWave = currentWave;
        var highestWave = 0;
        foreach (SpawnWave waveCheck in waves)
        {
            if (waveCheck.wave <= currentWaveNumber + 1 && waveCheck.wave > highestWave)
            {
                returnWave = waveCheck;
                highestWave = waveCheck.wave;
            }
        }
        if(returnWave == currentWave && !repeatWaves)
        {
            //Player has completed the winning wave

            if (winningWave)
            {
                WinningWaveWasBeaten = true;
                //Debug.Log(currentWave);
             // WinConditionMeet();
            }
            return (null);
        }
        else return (returnWave);
    }//finds next wave but does not increment counter

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
                    if(cam != null)
                    {
                        //old way of looking (worked in editor but not in build)
                        //var pos = cam.WorldToViewportPoint(spawnLocations[i].position);
                        //if (pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1 && pos.z >= 0) { Debug.Log("that spawnpoint is visible"); }

                        var rend = spawnLocations[i].GetComponent<Renderer>();
                        if(rend != null)
                        {
                            if (rend.isVisible) { }//Debug.Log("enemy spawn visible"); }
                            else { useable = true; }
                        }
                        else { Debug.Log("the wave spawn points each need a mesh renderer component to work even though they are invisible"); }                        
                    }
                    else { Debug.Log("can't find camera"); }
                }
            }
            while (!useable && escapeI <= 200);

            if(escapeI > 200) { Debug.Log("Can't find an active spawner"); return (Vector3.zero); }
            else return (spawnLocations[i].position);
        }
        else { Debug.Log("this should not return empty vector3"); return (new Vector3(0, 0, 0)); }
    }//chooses random from location list

    private GameObject GetRandomEnemy()
    {
        var size = currentWave.spawnPool.Count;
        if (size > 0)
        {
            var i = Random.Range(0, size);
            return (currentWave.spawnPool[i]);
        }
        else { Debug.Log("no enemy pool set up or empty"); return (null); }
    }//returns random enemy from current pool

    public void SetUpWave()
    {
        currentWave = FindNextWave();
        if (currentWave != null)
        {
            currentWaveMaxTime = currentWave.waveTime;
            currentWaveTime = currentWaveMaxTime;
            coolDownTimer = currentWave.coolTime;
            spawnMaxCount = currentWave.spawnNumber;
            careAboutEnemies = currentWave.careAboutEnemies;
            spawnCount = 0;
            currentWaveNumber++;
            WasThereAWaveChange = true;
            PlaySound(waveStartSound);
            hasPlayedCoolDownSound = false;
        }
        else
        {
            Debug.Log("no current wave or waves are done");
            counting = false;
            WasThereAWaveChange = false;
        }
    }//initializes wave variables used in this script

    private void SpawnEnemy()
    {
        var enemyType = GetRandomEnemy();
        var pos = GetRandomPos();

        if(pos != Vector3.zero)
        {
            DLC_EnemyManager eManage = new DLC_EnemyManager();
            eManage.e_Instance = Instantiate(enemyType, pos, transform.rotation);
            eManage.SetupAI(spawnLocations);

            var arrow = Instantiate(arrowObj, pos, Quaternion.identity);
            var arrowScript = arrow.GetComponent<ArrowPointerScript>();
            arrowScript.enemyTrans = eManage.e_Instance.transform;

            spawnCount++;
            if (careAboutEnemies || currentWave.addEnemyToCareList)
            {
                if(enemyType.name != "AI_Enemy"
                    && enemyType.name != "AI_Enemy 1"
                    && enemyType.name != "AI_EnemyRunAndGun"
                    && enemyType.name != "AI_EnemySniper"
                    && enemyType.name != "AI_EnemyWanderAndHit"
                    && enemyType.name != "AI_EnemyWander")
                {
                    var health = eManage.e_Instance.GetComponentInChildren<AMS_Health_Management>();
                    if (health != null) { enemiesSpawned.Add(health); }
                    else { Debug.Log("we are spawning enemies with no health component!"); }
                }
                else { }//don't want to track these as they lurk
            }
        }
    }//creates a random enemy at a random location and increments counter

 /* private void WinConditionMeet()
    {
        //Go to next scene
        AMS_UniversalFunctions.GoToResultsScreen(true);
    }
*/
    private void CleanList<T>(List<T> list)
    {
        for(var i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null || list[i].Equals(null)) { list.RemoveAt(i); }
        }
    }//removes nulls from list

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

    private void PlaySound(AudioClip sound)
    {
        var switcher = FindObjectOfType<AudioSwitcherScript>();
        if(switcher != null)
        {
            if(sound != null)
            {
                switcher.PlaySound(sound);
            }
            else
            {
                Debug.Log("no sound assigned in public var");
            }
        }
        else
        {
            Debug.Log("can't find audio switcher (should be on player object)");
        }
    }
}