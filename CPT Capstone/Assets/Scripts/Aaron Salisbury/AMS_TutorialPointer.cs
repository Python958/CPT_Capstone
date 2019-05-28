using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_TutorialPointer : MonoBehaviour
{
    public GameObject objectTryingToFind;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (objectTryingToFind != null)
        {
            arrow.SetActive(true);
            Vector3 ObjectTryingToFindPostion = new Vector3(objectTryingToFind.transform.position.x, transform.position.y, objectTryingToFind.transform.position.z);
            transform.LookAt(ObjectTryingToFindPostion);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
