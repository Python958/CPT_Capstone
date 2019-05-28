using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinder : MonoBehaviour
{
    public GameObject BossArrow;
    public GameObject Boss;
    public GameObject BossIndicator;
    bool BossSpawned;
    // Start is called before the first frame update
    void Start()
    {
        //Boss = GameObject.Find("AI_BossRunAndGun");
        BossIndicator = GameObject.Find("TargetIndicator");
        BossArrow = BossIndicator.transform.Find("BossArrow").gameObject;
        BossArrow.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        Boss = GameObject.Find("AI_BossRunAndGun(Clone)");
        BossSpawned = FindObjectOfType<FinalSpawn>().isSpawned;
        ArrowActive();
    }

    private void LateUpdate()
    {
        //Boss = GameObject.Find("AI_BossRunAndGun");
        if (Boss != null)
        {           
            Vector3 BossPosition = new Vector3(Boss.transform.position.x, transform.position.y, Boss.transform.position.z);
            transform.LookAt(BossPosition);
        }
    }

    private void ArrowActive()
    {
        if(BossSpawned == true)
        {
           // Debug.Log("A new enemy Approaches");
            BossArrow.SetActive(true);
        }
    }
}
