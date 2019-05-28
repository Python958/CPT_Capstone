using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_Ammo_Pickup : MonoBehaviour
{
    public int totalClips = 5;
    public int currentClips;
    private AMS_GunManagement playerAmmo;
    public string type;
    private GameObject note;
    private Text AmmoText;
    private bool collectable = false;
    public Material secondMat;

    private void Start()
    {
        if (type == "Unlock")
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                if (rend.materials[0].name.Contains("AmmoCrate1"))
                {
                    if(secondMat != null)
                    {
                        rend.material = secondMat;
                    }
                    else { Debug.Log("secondMat not assigned"); }
                }
            }
        }
        currentClips = totalClips;
        note = gameObject.GetComponentInChildren<Canvas>().gameObject;
        if(note == null) { Debug.Log("can't find canvas gameobject"); }
        AmmoText = note.GetComponentInChildren<Text>();

        note.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && collectable)
        {
            while (currentClips != 0 && playerAmmo.AddAmmo(type))
            {
                currentClips--;
            }
            if (currentClips == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerAmmo = other.gameObject.GetComponent<AMS_GunManagement>();
            if(playerAmmo.currentClips == playerAmmo.maxClips)
            {
                AmmoText.text = "Your ammo clips are full";
                collectable = false;
            }
            else
            {
                AmmoText.text = "Press E to grab Ammo";
                collectable = true;
            }

            note.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            collectable = false;
            note.SetActive(false);
        }
    }
}
