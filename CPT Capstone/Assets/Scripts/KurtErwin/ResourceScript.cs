using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    public int resourceValue;
    private AMS_ResourceController resourceController;
    public AudioClip resourceCollectSound;
    private float rayOffset;
    private GameObject note;
    private BaseHealth baseHealth;
    public float baseDepositRange;

    private void Start()
    {
        resourceController = FindObjectOfType<AMS_ResourceController>();
        if (resourceController == null) { Debug.Log("can't find resource controller object"); }

        //this is just to find the bottom of the object
        var capColl = gameObject.GetComponent<CapsuleCollider>();
        if(capColl != null) { rayOffset = (capColl.height / 2f) * gameObject.transform.localScale.y ; }
        else { rayOffset = 1f; }

        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if(note == null) { Debug.Log("can't find canvas gameObject"); }

        baseHealth = FindObjectOfType<BaseHealth>();
        if(baseHealth == null) { Debug.Log("basehealth not found in scene"); }
    }

    void Update()
    {
        var dist = Vector3.Distance(transform.position, baseHealth.transform.position);
        if(dist <= baseDepositRange)
        {
            AMS_ScoreController.increaseScore(resourceValue);
            //increment resource pool in here some where

            resourceController.currentResources += resourceValue;
            resourceController.addedResources.text = "+ " + resourceValue; //added GUI update
            {
                StartCoroutine("GuiTimer");
            }
            Destroy(gameObject);
        }
    }

    public GameObject ReturnCanvas()
    {
        return (note);
    }

    public IEnumerator GuiTimer ()
    {
        yield return new WaitForSeconds(3);
        resourceController.addedResources.text = "";
      //Debug.Log("resource added");
    }
}
