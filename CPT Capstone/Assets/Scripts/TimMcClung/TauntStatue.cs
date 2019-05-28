using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntStatue : MonoBehaviour
{
    public GameObject healthComponent;
    public float effectRange;

    // Start is called before the first frame update
    void Start()
    {
        if(effectRange <= 0f) { effectRange = 25f; }
    }

    // Update is called once per frame
    void Update()
    {
        if(healthComponent == null)
        {
            Destroy(gameObject);
        }
    }
}
