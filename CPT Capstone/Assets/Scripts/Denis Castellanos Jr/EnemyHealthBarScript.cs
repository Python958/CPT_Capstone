using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{

    public AMS_Health_Management health_Management;
    private Slider enemyHealthBar;
    public GameObject barObject;
    private float maxEnemyHealth;
    private float currentEnemyHealth;
    public float healthShowTimeMax;
    private float healthShowTimeCurrent;
    public bool hideHealthBar = false;
    private float lastEnemyHealth;
    //public Canvas ParentCanvas;


    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar = barObject.GetComponent<Slider>();

        var bill = barObject.AddComponent<BillboardScript>();
        bill.freezeX = false;
        bill.freezeY = false;
        bill.freezeZ = true;

        maxEnemyHealth = health_Management.maxHealth;
        lastEnemyHealth = health_Management.currentHealth;
        enemyHealthBar.value = lastEnemyHealth;

        if (hideHealthBar) { ChangeBarVis(false); }//hides health bar
    }

    // Update is called once per frame
    void Update()
    {
        if(hideHealthBar && healthShowTimeCurrent > 0f)
        {
            healthShowTimeCurrent -= Time.deltaTime;
            if(healthShowTimeCurrent <= 0f)
            {
                healthShowTimeCurrent = 0f;
                ChangeBarVis(false);
            }
        }

        currentEnemyHealth = health_Management.currentHealth;
        if (currentEnemyHealth != lastEnemyHealth)
        {
            lastEnemyHealth = currentEnemyHealth;
            enemyHealthBar.value = CalculateHealthLeft();

            if (hideHealthBar) { ChangeBarVis(true); }//show health bar
        }        
    }
    float CalculateHealthLeft()
    {
        var HealthLost = currentEnemyHealth / maxEnemyHealth;
        return (HealthLost);
    }

    private void ChangeBarVis(bool visible)
    {
        if (visible) { healthShowTimeCurrent = healthShowTimeMax; }
        if(barObject != null) { barObject.SetActive(visible); }
    }
}