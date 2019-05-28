using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_TutorialDoorControl : MonoBehaviour
{
    public float moveAmountHorizontal;      //total amount to move horizontal
    public float moveAmountVertical;        //total amount to move vertical
    public float timeOfMove;                //how long it should take to make move
    public GameObject objectToMove;         //actual door part 

    private Vector3 moveSpeed;
    private float currentMoveTime = 0f;

    private bool moving = false;

    private bool waitingMove = false;

    // Start is called before the first frame update
    void Start()
    {
        var moveHSpeed = moveAmountHorizontal / timeOfMove;
        var moveVSpeed = moveAmountVertical / timeOfMove;

        moveSpeed = new Vector3(moveHSpeed, moveVSpeed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingMove)
        {
            if (!moving)
            {
                moving = true;
                currentMoveTime = 0;
                var moveHSpeed = moveAmountHorizontal / timeOfMove;
                var moveVSpeed = moveAmountVertical / timeOfMove;
                moveSpeed = new Vector3(moveHSpeed, moveVSpeed, 0f);
                waitingMove = false;
            }
        }
        if (moving)
        {
            currentMoveTime += Time.deltaTime;
            if (timeOfMove > currentMoveTime)
            {
                objectToMove.transform.Translate(moveSpeed * Time.deltaTime);
            }
            else { moving = false; }
        }
    }

    public void MakeMove()
    {
        if (moving)
        {
            waitingMove = true;
        }
        else
        {
            moving = true;
            currentMoveTime = 0;
            var moveHSpeed = moveAmountHorizontal / timeOfMove;
            var moveVSpeed = moveAmountVertical / timeOfMove;
            moveSpeed = new Vector3(moveHSpeed, moveVSpeed, 0f);
        }
    }
}
