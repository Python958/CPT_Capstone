using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarScript : MonoBehaviour
{
    public AMS_Ammo_Pickup AmmoPickUP;
    public Slider AmmoDropBar;
    public float MaxClipsPerCrate;
    public float CurrentClips;

    // Start is called before the first frame update
    void Start()
    {
        MaxClipsPerCrate = AmmoPickUP.totalClips;
        AmmoDropBar.value = MaxClipsPerCrate;
    
    }

    // Update is called once per frame
    void Update()
    {
        CurrentClips = AmmoPickUP.currentClips;
        if (CurrentClips <= MaxClipsPerCrate)
        {
            AmmoDropBar.value = CalculateClipsLeft();
        }
    }

    float CalculateClipsLeft()
    {
        var ClipsUsed = CurrentClips / MaxClipsPerCrate;
        return (ClipsUsed);
    }
}
