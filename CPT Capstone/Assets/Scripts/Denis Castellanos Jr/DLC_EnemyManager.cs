//Tanks Unity3d Tutorial: https://unity3d.com/learn/tutorials/projects/tanks-tutorial/game-managers?playlist=20081
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace Game
{
    [Serializable]
    public class DLC_EnemyManager
    {
        public Color e_EnemyColor;
        public Transform e_Spawnpoint;
        public GameObject e_Instance; //stores the instance of the enemy
        public List<Transform> e_WayPointList;

        //reference enemy movement script here
        //reference enemy shooting scritp here
        private DLC_StateController e_StateController;

        public void SetupAI(List<Transform> wayPointList)
        {
            e_StateController = e_Instance.GetComponent<DLC_StateController>();
            e_StateController.SetupAI(true, wayPointList);

            //shooting

            //Get all the renders of the enemy.
            MeshRenderer[] renderers = e_Instance.GetComponentsInChildren<MeshRenderer>(); 

            for (int i = 0; i < renderers.Length; i++)
            {
                //renderers[i].material.color = e_EnemyColor;
            }

        }

        public void DisableControl()
        {
            //turn off the scripts
        }

        public void EnableControl()
        {
            //turn on scripts 
        }
        public void Rest()
        {
            //resets enemy spawns
            e_Instance.transform.position = e_Spawnpoint.position;
            e_Instance.transform.rotation = e_Spawnpoint.rotation;

            e_Instance.SetActive(false);
            e_Instance.SetActive(true);
        }
    }

}