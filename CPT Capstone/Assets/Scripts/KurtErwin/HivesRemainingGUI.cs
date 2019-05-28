using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HivesRemainingGUI : MonoBehaviour
{

    private int hivesRemaining;
    private int oldHivesRemaining;
    public List<GameObject> HivesGUI;
    
    // Start is called before the first frame update
    void Start()
    {
        SetHivesRemaining();
        oldHivesRemaining = hivesRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        SetHivesRemaining();
        if(oldHivesRemaining != hivesRemaining)
        {
            oldHivesRemaining = hivesRemaining;
            for (var i = 0; i < HivesGUI.Count; i++)
            {
                if(i >= hivesRemaining)
                {
                    HivesGUI[i].SetActive(false);
                }
            }
        }
    }

    private void SetHivesRemaining()
    {
        var theHives = FindObjectsOfType<AMS_Hive>();
        hivesRemaining = theHives.Length;
    }
}
