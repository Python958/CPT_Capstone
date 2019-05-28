using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    public GameObject Player;
    public GameObject Base;
    public GameObject Boss;
    public float DistanceBetweenBase;
    public float DistanceBetweenBoss;
   
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Base = GameObject.FindGameObjectWithTag("Base");
        if (Base = null) { Debug.Log("There is no base in the scene"); }      
    }

    // Update is called once per frame
    void Update()
    {
        Boss = GameObject.Find("Ai_Boss (rework)");
        Base = GameObject.FindGameObjectWithTag("Base");
        Distance();
        BossCheck();
    }

    void BossCheck()
    {
        Boss = GameObject.Find("Ai_Boss (rework)");

        if(Boss != null)
        {
            DistanceBetweenBoss = Vector3.Distance(Player.transform.position, Boss.transform.position);
            //   Debug.Log(DistanceBetweenBetween);
        }
    }

    void Distance()
    {
    if (Base != null)
    {
        DistanceBetweenBase = Vector3.Distance(Player.transform.position, Base.transform.position);
        //   Debug.Log(DistanceBetweenBase);
    }
    }
}
