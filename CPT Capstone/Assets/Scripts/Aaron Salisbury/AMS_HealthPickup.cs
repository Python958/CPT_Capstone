using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_HealthPickup : MonoBehaviour
{
    public int healAmount = 25;
    private AMS_Health_Management playerHealth;
    private GameObject note;
    private Text HealthText;
    private bool collectable = false;
    // Start is called before the first frame update
    void Start()
    {
        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if (note == null) { Debug.Log("can't find canvas gameobject"); }
        HealthText = note.GetComponentInChildren<Text>(); 
        note.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && collectable)
        {
            playerHealth.currentHealth += healAmount;
            if (playerHealth.currentHealth > playerHealth.maxHealth)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<AMS_Health_Management>();
            if (playerHealth.currentHealth != playerHealth.maxHealth)
            {
                HealthText.text = "Press E to grab Healthpack";
                collectable = true;
            }
            else
            {
                HealthText.text = "You have full health";
                collectable = false;
            }
            note.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            note.SetActive(false);
            collectable = false;
        }
    }
}
