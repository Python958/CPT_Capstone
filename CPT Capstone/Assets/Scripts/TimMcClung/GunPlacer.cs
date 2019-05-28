using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunPlacer : MonoBehaviour
{
    private AMS_ResourceController resourceControls;
    public PlaceableBehaviour placeable;
    public KeyCode place = KeyCode.E;
    public KeyCode select = KeyCode.Alpha4;
    public KeyCode cancel = KeyCode.Alpha2;
    public int maxPlaceable = 4;
    public int currentPlaced;
    PlaceableBehaviour currentSpawned;
    bool hasSpawned;

    private float timePassed;
    public int turretCost = 40;
    public Text gunText;
    public GameObject Turret1;

    //AMS not enough sound
    public AudioClip notEnoughSound;


    //Sets current placed turrets to 0
    void Start()
    {
        Turret1.SetActive(false);
        currentPlaced = 0;
        resourceControls = GameObject.FindObjectOfType<AMS_ResourceController>();
        
    }

    //checks player input and calls the appropriate function.
    void Update()
    {

        if (resourceControls.currentResources >= turretCost)
        {
            if (Turret1.activeSelf == false)
            {
                Turret1.SetActive(true);
               // Debug.Log("I can buy a turret");
            }
        }
        else
        {
            Turret1.SetActive(false);
        }


        if (!hasSpawned)
        {

        if (Input.GetKeyDown(select)) Spawn();
        }
        else
        {
            if (Input.GetKeyDown(place)) CanBuy();
            if (Input.GetKeyDown(cancel)) Cancel();

            ShowPreview();

        }
        if (gunText != null) { gunText.text = "Turrets " + currentPlaced + " / " + maxPlaceable; }
        else { Debug.Log("gun text is null and either hasn't been assigned or can't be found"); }
    }

    void Spawn()
    {

        currentSpawned = Instantiate(placeable);
        hasSpawned = true;

        if(resourceControls.currentResources < turretCost)
        {
            Cancel();
        }
    }

    void Place()
    {
        if (currentPlaced < maxPlaceable)
        {
            currentSpawned.Place();
            hasSpawned = false;
            placeable.gameObject.SetActive(true);
            currentPlaced++;
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }

    void Cancel()
    {
        Destroy(currentSpawned.gameObject);
        hasSpawned = false;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void ShowPreview()
    {
        currentSpawned.OnPreview();
    }
    void CanBuy()
    {
        if (resourceControls.currentResources >= turretCost)
        { 
            Place();
            resourceControls.currentResources -= turretCost;
        }
        else
        {
            //AMS_Cant buy
            gameObject.GetComponent<AudioSwitcherScript>().PlaySound(notEnoughSound);
        }
    }
}