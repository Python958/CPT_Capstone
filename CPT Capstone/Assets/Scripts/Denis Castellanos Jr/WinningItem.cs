using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningItem : MonoBehaviour
{
   
    public GameObject attachedObject = null;
    public float goalDistance;
    public float detachDistance;
    public float followSpeed;
    public float acceleration;
    private float currentSpeed;
    //public int resourceValue;
    //private AMS_ResourceController resourceController;
    public AudioClip resourceCollectSound;
    private bool grounded = true;
    private Vector3 prevPos;
    private float rayOffset;
    private GameObject note;
    private bool canvasShow = false;
    public bool respawn = false;
    private KeyCode grabResourceKey = KeyCode.E;
    private bool grabAble = false;
    //public int scoreGainedOnDeposit = 0;
    public bool WasTheWinningItemFound;

    private void Start()
    {
       // resourceController = FindObjectOfType<AMS_ResourceController>();
       // if (resourceController == null) { Debug.Log("can't find resource controller object"); }
        prevPos = transform.position;

        //this is just to find the bottom of the object
        var capColl = gameObject.GetComponent<CapsuleCollider>();
        if (capColl != null) { rayOffset = (capColl.height / 2f) * gameObject.transform.localScale.y; }
        else { rayOffset = 1f; }

        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if (note == null) { Debug.Log("can't find canvas gameObject"); }

        WasTheWinningItemFound = false;
    }

    void Update()
    {
        if (!grounded)
        {
            RaycastHit hit;
            var checkPos = new Vector3(transform.position.x, transform.position.y - rayOffset, transform.position.z);
            if (Physics.Raycast(checkPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.distance > .01)
                {
                    var amount = Mathf.Min(hit.distance, followSpeed);
                    if (amount == hit.distance) { grounded = true; Debug.Log("grounded"); }
                    var newPos = new Vector3(transform.position.x, transform.position.y - amount, transform.position.z);

                    transform.position = newPos;
                }
            }
        }
        if (attachedObject != null)
        {
            if (note.activeSelf) { note.SetActive(false); }

            var distToTarget = Vector3.Distance(transform.position, attachedObject.transform.position);
            if (distToTarget > goalDistance)
            {
                if (distToTarget > detachDistance)
                {
                    var playerScript = attachedObject.GetComponent<KE_MainPlayer_Script>();
                    attachedObject = null;
                }
                else
                {
                    if (currentSpeed != followSpeed) { currentSpeed = Mathf.Min(followSpeed, currentSpeed + acceleration); }
                    transform.position = Vector3.MoveTowards(transform.position, attachedObject.transform.position, currentSpeed);

                    RaycastHit hit;
                    var checkPos = new Vector3(transform.position.x, transform.position.y - rayOffset, transform.position.z);
                    if (Physics.Raycast(checkPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                    {
                        if (hit.distance > .01) { grounded = false; Debug.Log("airborne"); }
                    }
                }//this is where we move towards the player and check for grav effects
            }
            else
            {
                if (currentSpeed != 0f) { currentSpeed = 0f; } //Mathf.Max(0f, currentSpeed - (acceleration*3)); }
            }
        }
        else if (note.activeSelf != canvasShow) { note.SetActive(canvasShow); }

        if (grabAble)
        {
            if (Input.GetKeyDown(grabResourceKey))
            {
                var playerScript = FindObjectOfType<KE_MainPlayer_Script>();
                if (playerScript == null)
                {
                    Debug.Log("couldn't find player script on player object");
                }
                else
                {
                    if (!playerScript.carrying)
                    {

                        Debug.Log("you will have to find a new way of doing this as dragging has been changed");
                        /*
                        attachedObject = playerScript.gameObject;
                        playerScript.SetCarrying(true);
                        WasTheWinningItemFound = true;
                        Debug.Log("Player grabbed the Winning Item");
                        Debug.Log("attached");
                        */
                    }
                    else
                    {
                        Debug.Log("you will have to find a new way of doing this as dragging has been changed");
                        /*
                        attachedObject = null;
                        playerScript.SetCarrying(false);
                        WasTheWinningItemFound = false;
                        Debug.Log("detached");
                        */
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
        
            //var playerScript = attachedObject.GetComponent<KE_MainPlayer_Script>();
           
            if (respawn)
            {
                var newSpawner = FindFreeSpawner();
                if (newSpawner != null)
                {
                    transform.position = newSpawner.transform.position;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else { Destroy(gameObject); }
            SceneManager.LoadScene("ResultsScreen", LoadSceneMode.Single);
        }
        else if (other.gameObject.tag == "Player")
        {
            canvasShow = true;
            grabAble = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvasShow = false;
            grabAble = false;
        }
    }

    private GameObject FindFreeSpawner()
    {
        WinningItemSpawnerScript[] spawnList = GameObject.FindObjectsOfType<WinningItemSpawnerScript>();
        List<WinningItemSpawnerScript> spawnsEmptyList = new List<WinningItemSpawnerScript>();

        if (spawnList.Length > 0)
        {
            for (var i = 0; i < spawnList.Length; i++)
            {
                if (spawnList[i].ownedResource == null)
                {
                    var rend = spawnList[i].GetComponent<Renderer>();
                    if (rend != null)
                    {
                        if (rend.isVisible) { Debug.Log("visible"); }
                        else { spawnsEmptyList.Add(spawnList[i]); }
                    }
                    else { Debug.Log("a resource spawner needs a mesh render componenet on the main object even though it is invisible"); }

                }
            }

            if (spawnsEmptyList.Count > 0)
            {
                var randIndex = Random.Range((int)0, spawnsEmptyList.Count);
                return (spawnsEmptyList[randIndex].gameObject);
            }
            else
            {
                Debug.Log("no empty spawners in scene. This should not happen");
                return (null);
            }
        }
        else
        {
            Debug.Log("there are no spawners in scene. Something is wrong");
            return (null);
        }
    }

    public GameObject ReturnCanvas()
    {
        return (note);
    }
}
