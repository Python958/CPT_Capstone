using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    /*
     * This should be a lot smoother. Perhaps start off slow on the follow speed and ramp it up until a certain distance is maintained
     * Also the trigger volume is small enough that the player doesn't trigger it from too far away but when dropping they can sometime be out of the trigger volume so it doesn't register.
     * Maybe have a separate trigger volume size for when attached and when not.
     */

    [HideInInspector]
    public bool grounded = true;
    private float rayOffset;
    private GameObject note;
    private bool canvasShow = false;
    private KE_MainPlayer_Script player;

    private void Start()
    {
        //this is just to find the bottom of the object


        var capColl = gameObject.GetComponentInChildren<CapsuleCollider>();
        if(capColl != null) { rayOffset = (capColl.height / 2f) * capColl.gameObject.transform.localScale.y ; }
        else { rayOffset = 1f; Debug.Log("couldn't find capsule collider"); Debug.Log(gameObject.name); }

        var canvas = gameObject.GetComponentInChildren<Canvas>();
        if(canvas != null) { note = canvas.gameObject; }
        else { Debug.Log("can't find canvas gameObject for draggable"); note = null; }

        player = FindObjectOfType<KE_MainPlayer_Script>();
        if(player == null) { Debug.Log("can't find player"); }
    }

    void Update()
    {
        InRange(); //check if player is in range to grab

        if(!grounded) { GoToGround(); }
        
        if (note != null && note.activeSelf != canvasShow) { note.SetActive(canvasShow); }
    }

    private void InRange()
    {
        if(player != null)
        {
            var dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= player.grabRange && !player.carrying)
            {
                canvasShow = true;
            }
            else
            {
                canvasShow = false;
            }
        }
        else { Debug.Log("lost player somehow"); }
    }//just checks if object is in range of player

    private void GoToGround()
    {
        RaycastHit hit;
        var checkPos = new Vector3(transform.position.x, transform.position.y - rayOffset, transform.position.z);
        if (Physics.Raycast(checkPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.distance > .1)
            {

                var amount = hit.distance;
                //var amount = Mathf.Min(hit.distance, 1f);
                //if (amount == hit.distance) { grounded = true; Debug.Log("grounded"); }
                grounded = true;
                var newPos = new Vector3(transform.position.x, transform.position.y - amount, transform.position.z);
                transform.position = newPos;
            }
            transform.position = transform.position + Vector3.up * .1f;
        }
    }//this should set the object on the ground

    public GameObject ReturnCanvas()
    {
        return (note);
    }
}
