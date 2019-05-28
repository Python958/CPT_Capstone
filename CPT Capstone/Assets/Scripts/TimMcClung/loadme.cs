using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadme : MonoBehaviour
{
    public static loadme load;
    public bool initialLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        if (load == null)
        {
            DontDestroyOnLoad(this);
            load = this;
        }
        else if (load != this)
        {
            Destroy(gameObject);
        }
    }
}
