//Tanks Unity3d Tutorial https://unity3d.com/learn/tutorials/projects/tanks-tutorial/game-managers?playlist=20081
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class DLC_GameManager : MonoBehaviour
    {
        public GameObject[] e_EnemyPrefab;
        public DLC_EnemyManager[] e_Enemy;
        public List<Transform> wayPointsForAI;

        private void Start()
        {
            SpawnAllEnemies();
        }

        private void SpawnAllEnemies()
        {
            for (int i = 0; i < e_Enemy.Length; i++) //Setting up the enemy AI 
            {
                e_Enemy[i].e_Instance = Instantiate(e_EnemyPrefab[i], e_Enemy[i].e_Spawnpoint.position, e_Enemy[i].e_Spawnpoint.rotation) as GameObject;
                e_Enemy[i].SetupAI(wayPointsForAI);
            }
        }


        public void DisableControl()
        {
            for (int i = 0; i < e_Enemy.Length; i++)
            {
                e_Enemy[i].DisableControl();
            }
            //turn off the scripts
        }

        public void EnableControl()
        {

            for (int i = 0; i < e_Enemy.Length; i++)
            {
                e_Enemy[i].EnableControl();
            }
            //turn on scripts 
        }
        public void RestEnemyControl()
        {
            for (int i = 0; i < e_Enemy.Length; i++)
            {
                e_Enemy[i].Rest();
            }

            //resets enemy spawns
        }
    }

}