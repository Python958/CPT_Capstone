using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveHealthBarScript : MonoBehaviour
{
    public AMS_Health_Management Health_Management;
    public Image HiveHealthBarImage;
    public Image HiveHealthBarBackgroundImage;
    public float maxHiveHealth;
    public float currentHiveHealth;
    private float maxHiveHealthBarFill = 1f;
    private float HiveHealthDisplayAmount;

    // Start is called before the first frame update
    void Start()
    {
        HiveHealthBarImage.gameObject.SetActive(false);
        HiveHealthBarBackgroundImage.gameObject.SetActive(false);
        HiveHealthBarImage.fillAmount = maxHiveHealthBarFill;
        maxHiveHealth = Health_Management.maxHealth;
        currentHiveHealth = maxHiveHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HiveHealthBar();
      //  Debug.Log(currentHiveHealth);
    }

    public void HiveHealthBar()
    {
        currentHiveHealth = Health_Management.currentHealth;
        HiveHealthDisplayAmount = currentHiveHealth / maxHiveHealth;
        HiveHealthBarImage.fillAmount = HiveHealthDisplayAmount;
        if(currentHiveHealth < maxHiveHealth)
        {
            HiveHealthBarBackgroundImage.gameObject.SetActive(true);
            HiveHealthBarImage.gameObject.SetActive(true);
        }
    }
}
