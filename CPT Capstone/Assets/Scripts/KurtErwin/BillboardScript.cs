using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public Camera cam;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;

    // Start is called before the first frame update
    void Start()
    {
        if(cam == null) { cam = Camera.main; }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam != null)
        {
            Vector3 rotation = cam.transform.rotation.eulerAngles;
            if (freezeX) { rotation.x = 0; }
            if (freezeY) { rotation.y = 0; }
            if (freezeZ) { rotation.z = 0; }
            transform.rotation = Quaternion.Euler(rotation);
        }
        else { Debug.Log("can't find a camera"); }
    }
}
