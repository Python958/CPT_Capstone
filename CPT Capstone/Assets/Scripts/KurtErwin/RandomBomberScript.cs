using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBomberScript : MonoBehaviour
{
    private Vector2 size;
    public GameObject bombPrefab;
    public float explosionScale;
    public float fallSpeed;

    public float timeSeparationBase;        //the minimum time the drops will be separated
    public float timeSeparationVariable;    //the variable amount added to base time between drops
    private float timeNext;                 //the time before the next drop
    private float timeCurrent = 0f;              //current counter

    private Transform playerTrans;
    private float heightOffset = 17f;

    public bool targetPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        size.x = transform.localScale.x;
        size.y = transform.localScale.z;

        var rend = gameObject.GetComponent<MeshRenderer>();
        if (rend != null) { Destroy(rend); }
        else { Debug.Log("can't find mesh renderer on the object"); }

        var player = FindObjectOfType<KE_MainPlayer_Script>();
        if(player != null)
        {
            playerTrans = player.gameObject.transform;
            transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + heightOffset, playerTrans.position.z);
        }
        else { Debug.Log("can't find the player"); }

        GetTimeNext();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTrans != null)
        {
            transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + heightOffset, playerTrans.position.z);
        }
        else { Debug.Log("lost the player transform"); }

        if (timeCurrent >= timeNext)
        {
            if (DropBomb())
            {
                timeCurrent = 0f;
                GetTimeNext();
            }
            else { Debug.Log("bomb script was unable to find a spot to drop its bombs. Happens occasionally but should not happen a lot"); }
        }
        else { timeCurrent += Time.deltaTime; }
    }

    private void GetTimeNext()
    {
        timeNext = timeSeparationBase + Random.Range(0, timeSeparationVariable);
    }

    private bool DropBomb()
    {
        bool dropped = false;
        int counter = 300; //escape counter

        while(!dropped && counter > 0)
        {
            counter--;
            var pos = PickRandomSpot();
            //Find and place the box where the mouse is.

            RaycastHit hit;
            Ray ray = new Ray(pos, Vector3.down);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    GameObject tempBomb = Instantiate(bombPrefab, pos, Quaternion.identity);
                    var bombScript = tempBomb.GetComponent<AMS_DroppedBomb>();
                    if(bombScript != null)
                    {
                        GameObject tempExplosion = bombScript.explosionRadius;
                        if(tempExplosion != null)
                        {
                            tempExplosion.transform.localScale = tempExplosion.transform.localScale * explosionScale;
                        }
                        else { Debug.Log("something seriously wrong with bomb prefab 1"); }
                        bombScript.fallSpeed = fallSpeed;
                        bombScript.showShadow = true;
                    }
                    else { Debug.Log("something seriously wrong with bomb prefab 2"); }
                    return (true);
                }
            }
        }
        return (false);
    }

    private Vector3 PickRandomSpot()
    {
        var x = Random.Range(0, size.x) - size.x/2;
        var z = Random.Range(0, size.y) - size.y/2;

        if (targetPlayer) { x /= 5; z /= 5; }

        Vector3 position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        return (position);
    }
}
