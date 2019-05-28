using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public float moveAmountHorizontal;
    public float moveAmountVertical;
    public float timeToMove;
    public GameObject objectToMove;

    private Vector3 moveSpeed;
    private float currentMoveTime = 0f;

    private bool moving = false;
    public bool moved = false;
    private bool inTrigger = false;

    public AudioClip[] DoorSounds;
    public AudioSource DoorAudioSource;
    public int scoreGainedUponOpening = 20;
    // Start is called before the first frame update
    void Start()
    {
        var moveHSpeed = moveAmountHorizontal / timeToMove;
        var moveVSpeed = moveAmountVertical / timeToMove;

        moveSpeed = new Vector3(moveHSpeed, moveVSpeed, 0f);
        DoorAudioSource = gameObject.AddComponent<AudioSource>();
        DoorAudioSource.playOnAwake = false; //Turn off playOnAwake
        DoorAudioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            currentMoveTime += Time.deltaTime;
            if (timeToMove > currentMoveTime)
            {
                objectToMove.transform.Translate(moveSpeed * Time.deltaTime);
                objectToMove.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                moving = false;
            }
        }
        else if (!moved && inTrigger == true)
        {          
                OpenDoor();
                moved = true;            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            inTrigger = false;
        }
    }

    public void OpenDoor()
    {
        AMS_ScoreController.increaseScore(scoreGainedUponOpening);
        moving = true;
        moved = true;
       // DoorAudioSource.clip = DoorSounds[0];
       // DoorAudioSource.Play();

    }
}
