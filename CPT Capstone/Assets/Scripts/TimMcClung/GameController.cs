using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public static class GameController
{ 
    //Creates a .dat file in the project for saving purposes
    public static void Save (KE_MainPlayer_Script player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerInfo.dat";
        FileStream savefile = new FileStream(path, FileMode.Create);

        //After Binary setup, gives the ability to write to the disk.
        PlayerData data = new PlayerData(player);

        Debug.Log("Game Saved at " + path);
        Debug.Log("Player health saved as  " + data.health);
        Debug.Log("Player clips1 saved as  " + data.ammoClips1);
        Debug.Log("Player clips2 saved as  " + data.ammoClips2);
        Debug.Log("Player ammo1 saved as  " + data.ammo1);
        Debug.Log("Player ammo2 saved as  " + data.ammo2);
        Debug.Log("current level saved as  " + data.currentLevel);

        bf.Serialize(savefile, data);
        savefile.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerInfo.dat";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savefile = new FileStream(path, FileMode.Open);

            PlayerData data = bf.Deserialize(savefile) as PlayerData;
            savefile.Close();
            return data;           
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}



//Contains the Data for the players Information (Health Ammo ETC)
[Serializable]
public class PlayerData
{
    public int[] spawn;
    public float health;
    public int ammoClips1;
    public int ammoClips2;
    public int ammo1;
    public int ammo2;
    public int checkpoint;
    public bool unlocked;
    public int currentLevel;
    public bool playerLoad = true;
    public string boughtweapon;
    public int resources;
    public int altFireammo;
    public string currentGun;

    public PlayerData(KE_MainPlayer_Script player)
    {
        health = player.GetComponent<AMS_Health_Management>().currentHealth;
        ammoClips1 = player.GetComponent<AMS_GunManagement>().currentClips;
        ammoClips2 = player.GetComponent<AMS_GunManagement>().gun2currentClips;
        ammo1 = player.GetComponent<AMS_GunManagement>().currentAmmo;
        ammo2 = player.GetComponent<AMS_GunManagement>().gun2currentAmmo;
        unlocked = player.GetComponent<AMS_GunManagement>().unlocked;
        currentLevel = player.GetComponent<MainMenu>().levelIndex;
        altFireammo = player.GetComponent<AMS_GunManagement>().defaultAlternateFireAmmo;
        currentGun = player.GetComponent<AMS_GunManagement>().currentGun;
        var chSpawn = StaticCheckpointController.WhereToSpawn();
        spawn[0] = (int)chSpawn.x;
        spawn[1] = (int)chSpawn.y;
        spawn[2] = (int)chSpawn.z;

    }

    public PlayerData(AMS_BuyControls buy)
    {
        boughtweapon = buy.GetComponent<AMS_BuyControls>().boughtWeapon;
    }

    public PlayerData(AMS_ResourceController resource)
    {
        resources = resource.GetComponent<AMS_ResourceController>().currentResources;
    }
}
