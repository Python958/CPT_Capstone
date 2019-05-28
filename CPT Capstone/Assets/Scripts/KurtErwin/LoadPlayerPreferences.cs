using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class LoadPlayerPreferences : MonoBehaviour
{

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        if (audioMixer == null) { Debug.Log("You need to assign the MainAudioMixer in Tim's folder"); }
        else
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                audioMixer.SetFloat("volume", Mathf.Log10(PlayerPrefs.GetFloat("masterVolume")) * 20);
                   //  Mathf.Log10(PlayerPrefs.GetFloat("masterVolume")) * 20; 
            }
            else { Debug.Log("Couldn't find in player prefs"); }
            if (PlayerPrefs.HasKey("sfxVolume")) { audioMixer.SetFloat("sfxvolume", Mathf.Log10(PlayerPrefs.GetFloat("sfxVolume"))*20); }
            if (PlayerPrefs.HasKey("musicVolume")) { audioMixer.SetFloat("musicvolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume")) * 20); }
        }//sets up audio mixer

        if (PlayerPrefs.HasKey("quality")) { QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality")); }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            var isFull = PlayerPrefs.GetInt("fullscreen");
            Screen.fullScreen = Convert.ToBoolean(isFull);
        }

        if(PlayerPrefs.HasKey("screenWidth") && PlayerPrefs.HasKey("screenHeight"))
        {
            var width = PlayerPrefs.GetInt("screenWidth");
            var height = PlayerPrefs.GetInt("screenHeight");
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}
