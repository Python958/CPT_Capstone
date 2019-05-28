using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public enum AiStates { NEAREST, FURTHEST};

    public AiStates aiState = AiStates.NEAREST;

    TurretTracker m_tracker;
    TurretShooting m_shooter;
    RangeChecker m_range;

    // Use this for initialization
    void Start()
    {
        m_tracker = GetComponent<TurretTracker>();
        m_shooter = GetComponent<TurretShooting>();
        m_range = GetComponent<RangeChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_tracker || !m_shooter || !m_range)
            return;

        switch (aiState)
        {
            case AiStates.NEAREST:
                TargetNearest();
                break;
            case AiStates.FURTHEST:
                TargetFurthest();
                break;
        }
    }

    void TargetNearest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        float closestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {

            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist < closestDist)
            {
                curTarget = validTargets[i];
                closestDist = dist;
            }
        }
       

        m_tracker.SetTarget(curTarget);
        m_shooter.SetTarget(curTarget);
    }

    void TargetFurthest()
    {
        List<GameObject> validTargets = m_range.GetValidTargets();

        GameObject curTarget = null;
        float furthestDist = 0.0f;

        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            if (!curTarget || dist > furthestDist)
            {
                curTarget = validTargets[i];
                furthestDist = dist;
            }
        }

        m_tracker.SetTarget(curTarget);
        m_shooter.SetTarget(curTarget);
    }
}
