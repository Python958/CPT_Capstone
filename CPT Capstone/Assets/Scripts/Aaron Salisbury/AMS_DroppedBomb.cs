using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_DroppedBomb : MonoBehaviour
{
    private bool moveOn;
    public GameObject explosionRadius;
    public float fallSpeed = -3;

    public bool showShadow = false;
    private GameObject shadow;
    public GameObject shadowPrefab;
    private float shadowStartDist;
    [HideInInspector]
    public float shadowMinScale = 0f;
    [HideInInspector]
    public float shadowMaxScale = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        moveOn = true;

        if (showShadow)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    if (shadowPrefab != null)
                    {
                        fallSpeed = fallSpeed / 2;
                        shadow = Instantiate(shadowPrefab, hit.point, Quaternion.identity);
                        shadowStartDist = hit.distance;
                        shadow.transform.localScale = new Vector3(shadowMinScale, shadow.transform.localScale.y, shadowMinScale);
                    }
                    else { Debug.Log("you need to set the shadow prefab in the inspector or through code (difficult)"); }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveOn)
        {
            transform.position += new Vector3(0, fallSpeed, 0) * Time.deltaTime;

            if(shadow != null)
            {
                var dist = Vector3.Distance(transform.position, shadow.transform.position);
                var percentScale = (shadowStartDist - dist) / shadowStartDist;
                var scaleSet = Mathf.Lerp(shadowMinScale, shadowMaxScale, percentScale);
                shadow.transform.localScale = new Vector3(scaleSet, shadow.transform.localScale.y, scaleSet);
            }//makes the shadow grow as the bomb nears it
            else { Debug.Log("shadow was null"); }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        moveOn = false;
        explosionRadius.SetActive(true);

        if(shadow != null) { Destroy(shadow); }
    }
}
