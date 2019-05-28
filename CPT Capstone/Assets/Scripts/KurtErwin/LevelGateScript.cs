using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class LevelGateScript : MonoBehaviour
{
    public float moveAmountHorizontal;          //total amount to move horizontal
    public float moveAmountVertical;            //total amount to move vertical
    public float timeOfMove;                    //how long it should take to make move
    public GameObject objectToMove;             //actual door part
    Renderer DoorRenderer;

    /*no longer used
    public float percOfCap;                     //percentage of resource cap to open up with
    private float actualTarget;                 //actual resource target to open at
    */

    public List<GameObject> spawnPointsDisable; //list of spawn points to disable when this is opened
    public List<GameObject> spawnPointsEnable;  //list of spawn points to enable when this is opened
    private Vector3 moveSpeed;
    private float currentMoveTime = 0f;

    private bool moving = false;
    private bool moved = false;
    private AMS_ResourceController resourceCont;
    private ResourceScript ResourceScript;

    public bool DoorPurchaseTrigger = false;

    public AudioClip[] DoorSounds;
    public AudioSource DoorAudioSource;
    public int DoorCost = 100;
    public int scoreGainedUponOpening = 20;
    //public int DoorSoundPicked;
    public GameObject C4;
    public GameObject explosion;

    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        DoorRenderer = objectToMove.GetComponent<Renderer>();
        resourceCont = GameObject.FindObjectOfType<AMS_ResourceController>();
        if (resourceCont == null)
        {
            Debug.Log("ERROR: can't find resource controller");
            Debug.Log("Self Destructing!!!");
            Destroy(gameObject);
        }
        /*else
        {
            actualTarget = resourceCont.winConditionResources * percOfCap;
            if (percOfCap != 1f)
            {
                var component = GetComponentInChildren<LevelGateHelperScript>();
                if (component != null) { Destroy(component.gameObject); Debug.Log("Deleting mid gate trigger volume - see comments"); }//This is deleted because if you don't need the full resource amount to open it is probably not a level end
                else { Debug.Log("can't find trigger, maybe someone already deleted it"); }
            }
        }
        */
        var moveHSpeed = moveAmountHorizontal / timeOfMove;
        var moveVSpeed = moveAmountVertical / timeOfMove;

        moveSpeed = new Vector3(moveHSpeed, moveVSpeed, 0f);
        DoorAudioSource = gameObject.GetComponent<AudioSource>();
        DoorAudioSource.playOnAwake = false; //Turn off playOnAwake
        DoorAudioSource.playOnAwake = false;
       // DoorAudioSource.outputAudioMixerGroup.audioMixer.name = "SfxMixer";
        ResourceScript = GameObject.FindObjectOfType<ResourceScript>();

        SetSpawnPoints(spawnPointsEnable, false);//this sets whatever the door would enable to disabled

        /*  for (int i = 0; i < DoorSounds.Length; i++)
          {
              DoorSoundPicked++;
          }*/

    }

    // Update is called once per frame
    void Update()
    {
        CanPurchaseDoor();
        if (moving)
        {
            currentMoveTime += Time.deltaTime;
            if (timeOfMove > currentMoveTime)
            {
                objectToMove.transform.Translate(moveSpeed * Time.deltaTime);
            }
            else
            {
                moving = false;
                RemoveNavMeshObstacles();
            }
        }
        /*  else if (!moved)
          {
              if (resourceCont != null)
              {
                  if (resourceCont.currentResources >= actualTarget) { moving = true; moved = true; }
              }
              else
              {
                  Debug.Log("ERROR: can't find resource controller");
                  Debug.Log("Self Destructing!!!");
                  Destroy(gameObject);
              }
         }*/
        else if (!moved && DoorPurchaseTrigger == true && Input.GetKeyDown (KeyCode.E))
        {

            if (resourceCont.currentResources >= DoorCost)
            {
                C4.SetActive(true);
                explosion.SetActive(true);
                OpenDoor();
                resourceCont.currentResources -= DoorCost;
                resourceCont.addedResources.text = "- " + DoorCost;
                Debug.Log("The Player opened the door");
                moved = true;
                GetComponent<Collider>().enabled = false;
                Invoke("TextClear", 2);
                C4.SetActive(false);
                explosion.SetActive(false);

            }
            else if (resourceCont.currentResources < DoorCost)
            {
                resourceCont.addedResources.text = "Not Enough Resources";
                DoorAudioSource.clip = DoorSounds[2];
                DoorAudioSource.Play();
                Debug.Log("You need more resources to unlock");
            }
            
        }
    }

    public void MakeMove() { moving = true; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            resourceCont.addedResources.text = "Press E to unlock the door for " + DoorCost + " resources" ; 
            DoorPurchaseTrigger = true;
            //Debug.Log("The Player entered the Gate Buying Zone");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DoorPurchaseTrigger = false;
            //Debug.Log("The Player left the Gate Buying Zone");
            resourceCont.addedResources.text = "";
        }
    }

    public void OpenDoor()
    {
        AMS_ScoreController.increaseScore(scoreGainedUponOpening);
        moving = true;
        moved = true;
        DoorAudioSource.clip = DoorSounds[0];
        DoorAudioSource.Play();
        SetSpawnPoints(spawnPointsDisable, false);
        SetSpawnPoints(spawnPointsEnable, true);
    }

    private void SetSpawnPoints(List<GameObject> spawnList, bool isEnabled)
    {
        if(spawnList.Count > 0)
        {
            for(var i = 0; i < spawnList.Count; i++)
            {
                if(spawnList[i] != null)
                {
                    spawnList[i].SetActive(isEnabled);
                }
            }
        }
    }//enables and disabled objects for the scene

    private void TextClear()
    {
        resourceCont.addedResources.text = "";
    }

    private void CanPurchaseDoor()
    {
        if (resourceCont.currentResources >= DoorCost)
        {
            DoorRenderer.material = materials[0];
        }
        else if (resourceCont.currentResources < DoorCost)
        {
            DoorRenderer.material = materials[1];
        }
    }

    private void RemoveNavMeshObstacles()
    {
        NavMeshObstacle[] navMeshObstacles = GetComponentsInChildren<NavMeshObstacle>();
        foreach(NavMeshObstacle obs in navMeshObstacles) { Destroy(obs); }
    }
}
