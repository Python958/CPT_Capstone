using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    public List<string> tags;
    public float maxDistance;
    List<GameObject> m_targets = new List<GameObject>();
    List<GameObject> m_possibleTargets = new List<GameObject>();
    public float fieldOfViewAngle = 110f;



    void Update()
    {
        Vector3 direction = transform.position + transform.position;
        Debug.DrawRay(transform.position, Vector3.forward * maxDistance);
        float angle = Vector3.Angle(direction, transform.forward);

        //AMS Edit
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (m_targets[i] != null)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, m_targets[i].transform.position, out hit))
                {
                    if (hit.collider.tag != "Default_Enemy")
                    {
                        m_possibleTargets.Add(m_targets[i]);
                        m_targets.Remove(m_targets[i]);
                    }
                }
            }

        }
        for (int i = 0; i < m_possibleTargets.Count; i++)
        {
            if (m_possibleTargets[i] != null)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, m_possibleTargets[i].transform.position, out hit))
                {
                    if (hit.collider.tag == "Default_Enemy")
                    {
                        m_targets.Add(m_possibleTargets[i]);
                        m_possibleTargets.Remove(m_possibleTargets[i]);
                    }
                }
            }
        }

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            Ray forwardRay = new Ray(transform.position, Vector3.forward);

            if (Physics.Raycast(forwardRay, out hit, maxDistance))
            {

                if (hit.collider.tag == "Default_Enemy")
                {
                    // Debug.Log("I have a target");
                }

            }
            else
            {               
                //Debug.Log("No Target");
            }
        }
    }
    void OnTriggerEnter (Collider other)
    {
        bool invalid = true;
      //  Debug.Log("I have found an enemy");
        for (int i = 0; i < tags.Count; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                invalid = false;
                break;
            }
        }

        if (invalid)
            return;

        m_targets.Add(other.gameObject);
    }
    void OnTriggerExit (Collider other)
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (other.gameObject == m_targets[i])
            {
                m_targets.Remove(other.gameObject);
                return;
            }
        }
        for (int i = 0; i < m_possibleTargets.Count; i++)
        {
            if (other.gameObject == m_possibleTargets[i])
            {
                m_possibleTargets.Remove(other.gameObject);
                return;
            }
        }
    }
    public List<GameObject> GetValidTargets()
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (m_targets[i] == null)
            {
                m_targets.RemoveAt(i);
                i--;
            }
        }

        return m_targets;
    }

    public bool InRange(GameObject go)
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (go == m_targets[i])
                return true;
        }

        return false;
    }
}
/*
 *          bool invalid = true;

         for (int i = 0; i < tags.Count; i++)
         {
             if (other.CompareTag(tags[i]))
             {
                 invalid = false;
                 break;
             }
         }

         if (invalid)
             return;

         m_targets.Add(other.gameObject);
*/
/*void Update() (Backup)
{
    Vector3 direction = transform.position + transform.position;
    Debug.DrawRay(transform.position, Vector3.forward * maxDistance);
    float angle = Vector3.Angle(direction, transform.forward);

    if (angle < fieldOfViewAngle * 0.5f)
    {
        RaycastHit hit;
        Ray forwardRay = new Ray(transform.position, Vector3.forward);

        if (Physics.Raycast(forwardRay, out hit, maxDistance))
        {

            if (hit.collider.tag == "Default_Enemy")
            {
                m_targets.Add(gameObject);
                InRange(gameObject);
                // Debug.Log("I have a target");
            }

        }
        else
        {
            m_targets.Remove(gameObject);
            //Debug.Log("No Target");
        }
    }
}*/