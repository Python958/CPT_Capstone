using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    // public GameObject LevelGate;
    // public GameObject Door;
    // private AMS_ResourceController resourceController;
    public GameObject Base;
    public GameObject TargetIndicator;
    public GameObject Arrow;
    public WinningItemSpawnerScript WinningItemSpawnerScript;
  //  public WinningItem WinningItemScript;
    private KE_MainPlayer_Script MainPlayerScript;

    private void Start()
    {
        /* LevelGate = GameObject.Find("LevelGate");
         if(LevelGate != null)
         {
             Door = LevelGate.transform.Find("Door").gameObject;
         }
         else { Debug.Log("cannot find a level gate"); }
        */
        //resourceController = FindObjectOfType<AMS_ResourceController>();
        TargetIndicator = GameObject.Find("TargetIndicator");
        Arrow = TargetIndicator.transform.Find("Arrow").gameObject;
        WinningItemSpawnerScript = FindObjectOfType<WinningItemSpawnerScript>();
   //     WinningItemScript = WinningItemSpawnerScript.GetComponent<WinningItem>();
        MainPlayerScript = FindObjectOfType<KE_MainPlayer_Script>();
        Base = GameObject.Find("Base");
        Arrow.SetActive(false);
    }

    void Update()
    {
    //    IndicatorTest();
    }

    private void LateUpdate()
    {
        if (Base != null)
        {
            Vector3 BasePosition = new Vector3(Base.transform.position.x, transform.position.y, Base.transform.position.z);
            transform.LookAt(BasePosition);
            //Vector3 DoorPosition = new Vector3(Door.transform.position.x, transform.position.y, Door.transform.position.z);
            //transform.LookAt(DoorPosition);
        }
    }

 /*   private void IndicatorTest()
    {
        if (WinningItemScript.WasTheWinningItemFound == true) 
        {
            Debug.Log("The player has grabbed the winning object");
            Arrow.SetActive(true);
        }
        else if (WinningItemScript.WasTheWinningItemFound == false)
        {
            Arrow.SetActive(false);
           // Debug.Log("The player has dropped the winning object");
        }
    }
*/
}