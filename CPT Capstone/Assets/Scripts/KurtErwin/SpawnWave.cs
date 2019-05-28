using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWave : MonoBehaviour
{
    public bool trapedFloor;
    public bool waveOfFire;
    public int wave;
    public float waveTime;
    public float coolTime;
    public int spawnNumber;
    public List<GameObject> spawnPool;
    public GameObject trapFloor;
    public GameObject fireFloor;
    public bool careAboutEnemies = true;
    public bool addEnemyToCareList = false;
}