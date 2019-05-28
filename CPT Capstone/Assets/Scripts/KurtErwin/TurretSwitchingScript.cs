using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSwitchingScript : MonoBehaviour
{
    private KeyCode grabResourceKey = KeyCode.E;
    private FriendlyTurret friendly;
    private EnemyTurret enemy;
    private DragScript dragScript;
    public GameObject turretStatus;
    public Light turretLight;

    // Start is called before the first frame update
    void Start()
    {
        friendly = GetComponentInChildren<FriendlyTurret>();
        enemy = GetComponentInChildren<EnemyTurret>();
        dragScript = GetComponent<DragScript>();
        friendly.enabled = false;
        enemy.enabled = true;

        // **Turrets needs to be assigned in the prefab hiearchy due to multiple turrets in the scene.**
        // turretStatus = GameObject.Find("DragTurret/TurretAI/Brains/Antennae/TurretStatus"); 
        turretLight = turretStatus.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(grabResourceKey))
        {
            var playerScript = FindObjectOfType<KE_MainPlayer_Script>();
            if (playerScript == null)
            {
                Debug.Log("couldn't find player script on player object");
            }
            else
            {
                if (playerScript.carrying && playerScript.carriedObj == gameObject)
                {
                    Debug.Log("switched turret alliance");
                    turretLight.color = Color.green;
                    friendly.enabled = true;
                    enemy.enabled = false;
                    Destroy(this);
                }
            }
        }
    }
}
