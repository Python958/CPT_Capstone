using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherLevelScript : MonoBehaviour
{

    private GameObject randomDropper;
    public GameObject dropperPrefab;

    [Header("Bomb Variables")]
    public float dropTime;
    public float dropTimeVariance;
    public float explosionScale;
    public float fallSpeed;

    [Header("Launcher Variables")]
    public float range;



    // Start is called before the first frame update
    void Start()
    {
        randomDropper = Instantiate(dropperPrefab, Vector3.zero, Quaternion.identity);
        randomDropper.transform.localScale = new Vector3(2f, 1f, 2f);
        var randomDropScript = randomDropper.GetComponent<RandomBomberScript>();
        if(randomDropScript != null)
        {
            randomDropScript.timeSeparationBase = dropTime;
            randomDropScript.timeSeparationVariable = dropTimeVariance;
            randomDropScript.explosionScale = explosionScale;
            randomDropScript.fallSpeed = fallSpeed;
        }
        else { Debug.Log("random dropper doesn't have drop script"); }
    }

    // Update is called once per frame
    void Update()
    {
        var player = FindObjectOfType<KE_MainPlayer_Script>();
        if(player != null)
        {
            var pTrans = player.gameObject.transform;
            var dist = Vector3.Distance(pTrans.position, transform.position);
            if (dist <= range)
            {
                if (!randomDropper.activeSelf)
                {
                    randomDropper.SetActive(true);
                }
            }//inrange so activate dropper
            else
            {
                if (randomDropper.activeSelf)
                {
                    randomDropper.SetActive(false);
                }
            }//out of range so deactivate dropper
        }
        else { }
    }
}
