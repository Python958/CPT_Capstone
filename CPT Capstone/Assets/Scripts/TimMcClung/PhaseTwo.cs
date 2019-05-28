using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTwo : MonoBehaviour
{ 
    public bool phaseTwo;
    public float health; //health to know when p2 starts
    public int ptMDamage; //Melee damage for Phase 2
    public int ptRDamage; //Ranged damage for P2
    public float ptMTime; //Melee cd in p2
    public float ptRTime; //Ranged time in p2
    public GameObject HeadE;
    Renderer EyeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        EyeRenderer = HeadE.GetComponent<Renderer>();
        phaseTwo = true;
    }

    // Update is called once per frame
    void Update()
    {
        //find components to be set in Stage 2 (Half health)
        health = GetComponent<AMS_Health_Management>().currentHealth;
        ptMDamage = FindObjectOfType<TempDamagePlayerVolume>().damageAmount;
        ptRDamage = FindObjectOfType<EnemyGun>().bulletDamage;

        if (health <= 750)
        {
            StageTwo();
        }
    }

    void StageTwo()
    {
        if (phaseTwo == true)
        {
            EyeRenderer.material.color = Color.magenta;
            phaseTwo = false;
        }
        //Debug.Log("Plus ULTRA!");

        ptMDamage = 90;
        ptRDamage = 60;
        ptMTime = 1f;
        ptRTime = .3f;
    }
}
