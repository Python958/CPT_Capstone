using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloorController : MonoBehaviour
{
    public GameObject fireFloor;
    bool spawnFire = false;
    public float fireTimer = 15;
    bool inTrigger = false;
    private GameObject note;
    private bool canvasShow = false;
    public GameObject PreviewFire;
    public bool preview;
    // Start is called before the first frame update
    void Start()
    {
        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if (note == null) { Debug.Log("can't find canvas gameObject"); }
        if (fireFloor == null)
        {
            Debug.Log("FireFloor not found, did someone drop some water?");
        }
        if(fireFloor != null)
        {
            fireFloor.SetActive(false);
        }
        if(PreviewFire != null)
        {
            PreviewFire.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (inTrigger == true && Input.GetKeyDown(KeyCode.E))
        {
            spawnFire = true;
            Debug.Log("We don't need no water");           
        }
        if (spawnFire == true)
        {
            fireTimer -= Time.deltaTime;
            fireFloor.SetActive(true);
            if (fireTimer <= 0)
            {
                spawnFire = false;
                fireFloor.SetActive(false);
                fireTimer = 15;
            }

        }
        if (preview == true)
        {
            if(PreviewFire != null)
            {
                PreviewFire.SetActive(true);
            }
        }
        if(preview == false || spawnFire == true)
        {
            PreviewFire.SetActive(false);
        }
        if(canvasShow == true)
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
        Debug.Log("Needing some fire?");       
        inTrigger = true;
        canvasShow = true;
        preview = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
        canvasShow = false;
        Debug.Log("Maybe next time then");
        preview = false;
    }
}
