using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSpawnScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject ownedResource;
    public GameObject resObject;
    public float resourceRespawnRate; //number of seconds before resource respawns
    private float resourceTimer = 0f;
    private bool alarmSet = true;
    public int resourceValue;
    private float baseMushroomSize = 1.6f;

    public Texture[] textures;
    public Sprite[] miniMapIcons;

    private void Start()
    {
        resourceValue = resourceValue == 0 ? 50 : resourceValue;
    }

    // Update is called once per frame
    void Update()
    {
        TrySpawnResource();
    }

    public void TrySpawnResource()
    {
        if (resourceTimer <= 0f)
        {
            if (ownedResource == null)
            {
                if (!alarmSet)
                {
                    resourceTimer = resourceRespawnRate;
                    alarmSet = true;
                }
                else
                {
                    float scaleAmount = Mathf.Max(resourceValue - 25, 0) / (100f - 25f);
                    scaleAmount = Mathf.Min(scaleAmount, 1f);
                    var rScale = Mathf.Lerp(1f, 2f, scaleAmount);

                    var hOffset = (rScale * baseMushroomSize) / 2 - baseMushroomSize / 2;
                    var newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + hOffset, gameObject.transform.position.z);
                    ownedResource = Instantiate(resObject, newPos, gameObject.transform.rotation);

                    var modelTrans = ownedResource.transform.Find("Mushroom1");
                    if (modelTrans != null) { modelTrans.localScale = new Vector3(rScale, rScale, rScale); }
                    var rend = ownedResource.GetComponentInChildren<Renderer>();
                    if(rend != null)
                    {
                        int index = (int)Mathf.Round(Mathf.Lerp(0, 3, scaleAmount));
                        rend.material.SetTexture("_MainTex", textures[index]);
                        ownedResource.GetComponentInChildren<Image>().sprite = miniMapIcons[index];
                    }
                    
                    var resScript = ownedResource.GetComponent<ResourceScript>();
                    resScript.resourceValue = resourceValue;
                    alarmSet = false;
                }
            }
        }
        else { resourceTimer -= Time.deltaTime; }
    }
}
