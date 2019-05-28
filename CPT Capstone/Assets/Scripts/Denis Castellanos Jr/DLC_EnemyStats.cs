// Leveraged from https://unity3d.com/learn/tutorials/topics/navigation/finite-state-ai-delegate-pattern?playlist=17105 Tutorial
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Enemy Stats")] // Can be accessed under DLC_DefaultEnemyStats
public class DLC_EnemyStats : ScriptableObject
{
    public float walkSpeed = 1;
    public float runSpeed = 14;

    public float lookDistance = 40f;
    public float lookSphereCastRadius = 1f;
    public float lookConeOfView = 90f;          //this is the number of degrees to either side of center that the enemy looks when the player is on top of them. It is attenuated based on distance

    public float attackRange = 1f;
    public float attackSpeed = 1f;
    public float attackForce = 15f;
    public int attackDamage = 50;

    public float patrolUpdateSpeed = 0f;                //this is for when they aren't chasing the player and should be really low
    public float chaseUpdateSpeed = 1f;                 //this is for when they are chasing the player and should be fairly high
    public float searchLength = 4f;
    public float searchTurnSpeed = 120f;

    [Header("Only used by boss")]
    public GameObject ForceField;

    public bool tauntWorks = true;
    //[HideInInspector]
    public bool usedShield = false;                     //so the boss only uses the shield once (not used for regular enemies)

    public BaseHealth baseScript;                       //used so the AI doesn't need to search for these over and over
    public KE_MainPlayer_Script playerScript;
    public bool checkedForBase = false;
}