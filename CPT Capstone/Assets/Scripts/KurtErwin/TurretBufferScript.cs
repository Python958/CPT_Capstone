using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBufferScript : MonoBehaviour
{
    public float range;
    public float rotationBuff = 300f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var worldTurrets = FindObjectsOfType<FriendlyTurret>();

        if(worldTurrets.Length > 0)
        {
            foreach (FriendlyTurret turret in worldTurrets)
            {
                var dist = Vector3.Distance(turret.gameObject.transform.position, gameObject.transform.position);
                if(dist <= range)
                {
                    if (turret.turretRotationBuff < rotationBuff) { turret.turretRotationBuff = rotationBuff; }
                    turret.leading = true;
                    turret.partTimer = 1;
                }
            }
        }
    }
}
