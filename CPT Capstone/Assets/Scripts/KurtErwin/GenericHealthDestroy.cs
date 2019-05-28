using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this simply looks for a health component and destroys only attached object when health is zero
//made for removing the "E" when the enemy dies but could be used for other things
public class GenericHealthDestroy : MonoBehaviour
{
    public GameObject targetObj;
    private AMS_Health_Management healthScript;
    
    // Start is called before the first frame update
    void Start()
    {
        if(targetObj == null) { targetObj = gameObject; }

        var oldestParent = targetObj.transform;
        while(oldestParent.parent != null) { oldestParent = oldestParent.parent; }

        healthScript = oldestParent.GetComponentInChildren<AMS_Health_Management>();
    }//just finds the health script to watch

    // Update is called once per frame
    void Update()
    {
        if(healthScript == null || healthScript.currentHealth <= 0) { Destroy(gameObject); }
    }
}
