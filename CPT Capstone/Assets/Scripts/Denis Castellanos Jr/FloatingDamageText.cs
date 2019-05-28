using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    public float FloatingTextDestroyTimer = 1.3f;
    public Vector3 Offset = new Vector3(0, 5, 0);
    public Vector3 RandomizeLocation = new Vector3(1, 0, 1);

    private void Start()
    {
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(RandomizeLocation.x, RandomizeLocation.x),Random.Range(RandomizeLocation.y, RandomizeLocation.y),
        Random.Range(RandomizeLocation.z, RandomizeLocation.z));
        Destroy(gameObject, FloatingTextDestroyTimer);
        
    }

}

