using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game; //access the namespace

public class DLC_StateController : MonoBehaviour
{
    public DLC_State currentState;
    public DLC_EnemyStats enemyStats;
    public Transform eyes;
    public DLC_State RemainSate;
    public NavMeshAgent navMeshAgent;
    public Transform chaseTarget;
    public Transform player;
    public float meleeDistance = 10f;
    //public Transform runAway;
    public float stateTimeElapsed;
    public bool isSeen;

    public List<Transform> wayPointList;
    public int nextWayPoint;
    public bool aiActive = false;

    private void Awake()
    {
        //shooting here
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyStats.usedShield = false;
    }

    public void SetupAI(bool aiActivationFromEnemyManager, List<Transform> wayPointsFromEnemyManger)
    {
        wayPointList = wayPointsFromEnemyManger;
        aiActive = aiActivationFromEnemyManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    private void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    private void OnDrawGizmos() //Draw things in the scene view that doesn't get displayed in the gameview
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor; //sets color of gizmos
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }

    public void TransitionToState(DLC_State nextState)
    {
        if (nextState != RemainSate)
        {
            currentState = nextState;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitStae()
    {
        stateTimeElapsed = 0;
    }

}
