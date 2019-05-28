using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_SpikeTrap : MonoBehaviour
{
    public GameObject spikes;

    public float cooldown;
    private float currentCooldown;
    public float deployTime;
    private float currentDeployTime;
    public int spikeDamage = 50;
    private List<GameObject> alreadyHit;
    [HideInInspector]
    public AMS_BuyControls buyControls;

    //Modes are Active, Cooldown, Deployed
    private string mode = "Active";
    private Collider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        alreadyHit = new List<GameObject>() { gameObject };
        buyControls = FindObjectOfType<AMS_BuyMenuPlayer>().buyMenuGUI.GetComponent<AMS_BuyControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == "Cooldown")
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
            {
                SwitchModes("Active");
            }
        }
        if (mode == "Deployed")
        {
            currentDeployTime -= Time.deltaTime;
            if (currentDeployTime < 0)
            {
                SwitchModes("Cooldown");
            }
        }
    }

    private void SwitchModes(string modeChange)
    {
        if (modeChange == "Active")
        {
            boxCollider.enabled = true;
            spikes.transform.localPosition = new Vector3(0, -.3f, 0);
        }
        if (modeChange == "Deployed")
        {
            alreadyHit = new List<GameObject>() { gameObject };
            currentDeployTime = deployTime;
            boxCollider.enabled = false;
            boxCollider.enabled = true;
            spikes.transform.localPosition = new Vector3(0, 1, 0);
        }
        if (modeChange == "Cooldown")
        {
            boxCollider.enabled = false;
            spikes.transform.localPosition = new Vector3(0, -1, 0);
            currentCooldown = cooldown;
        }
        mode = modeChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Does it have health
        if (other.GetComponent<AMS_Health_Management>())
        {
            //Active state
            if (mode == "Active")
            {
                SwitchModes("Deployed");
            }
            if (mode == "Deployed")
            {
                if (!alreadyHit.Contains(other.gameObject))
                {
                    if (other.tag == "Player")
                    {
                        if (GetPlayerDamage() > 0)
                        {
                            other.GetComponent<AMS_Health_Management>().TakeDamage(GetPlayerDamage(), "Trap");
                        }
                    }
                    else
                    {
                        other.GetComponent<AMS_Health_Management>().TakeDamage(spikeDamage, "Trap");
                    }
                    alreadyHit.Add(other.gameObject);
                }
            }
        }
    }

    private int GetPlayerDamage()
    {
        if (buyControls)
        {
            if (buyControls.levelInts[7] == 1)
            {
                return spikeDamage;
            }
            if (buyControls.levelInts[7] == 2)
            {
                return spikeDamage / 2;
            }
            if (buyControls.levelInts[7] == 3)
            {
                return 0;
            }
        }
        return spikeDamage;
    }

}
