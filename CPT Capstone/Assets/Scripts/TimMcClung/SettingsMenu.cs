using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class SettingsMenu : MonoBehaviour
{
    //Sets Reference to the Master Audio Mixer
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public Slider mainSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider voiceSlider;

    public Dropdown qualityDrop;
    public Toggle fullscreenToggle;

    public AudioClip[] testingNoises;
    private AudioSource audioSourceForTesting;

    //Creates an array for resolutions
    Resolution[] resolutions;

    //Sets a reference for the array to current available screen resolutions
    void Start()
    {
        if(resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        if (mainSlider != null)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                mainSlider.value = PlayerPrefs.GetFloat("masterVolume");
            }
            else
            {
                mainSlider.value = 1f;
            }
        }
        
        if(sfxSlider != null)
        {
            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            }
            else
            {
                sfxSlider.value = .5f;
            }
        }

        if(musicSlider != null)
        {
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            }
            else
            {
                musicSlider.value = .5f;
            }
        }

        if(voiceSlider != null)
        {
            if (PlayerPrefs.HasKey("voiceVolume"))
            {
                voiceSlider.value = PlayerPrefs.GetFloat("voiceVolume");
            }
            else
            {
                voiceSlider.value = 1f;
            }
        }

        if (qualityDrop != null)
        {
            qualityDrop.value = QualitySettings.GetQualityLevel();
        }
        if(fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    private void Update()
    {
        //UpdateAudioSliders();
    }

    //Allows for main volume control
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        TestVolume("Master");
        PlayerPrefs.SetFloat("masterVolume",volume);
        PlayerPrefs.Save();
    }

    //Allows for SFX volume control
    public void SfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxvolume", Mathf.Log10(volume) * 20);
        TestVolume("SfxMixer");
        PlayerPrefs.SetFloat("sfxVolume",volume);
        PlayerPrefs.Save();
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("musicvolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void VoiceVolume(float volume)
    {
        audioMixer.SetFloat("voiceVolume", Mathf.Log10(volume) * 20);
        TestVolume("Voice");
        PlayerPrefs.SetFloat("voiceVolume", volume);
        PlayerPrefs.Save();
    }

    //Allows for graphics control
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
        PlayerPrefs.Save();
        ClickSound();
    }

    //Allows player to set their game to fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", Convert.ToInt32(isFullscreen));
        PlayerPrefs.Save();
        ClickSound();
    }

    //Allows for Resolution manipulation
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("screenWidth", resolution.width);
        PlayerPrefs.SetInt("screenHeight", resolution.height);
        PlayerPrefs.Save();
        ClickSound();
    }

    private void UpdateAudioSliders()
    {
        //main volume
        float tempValue;
        audioMixer.GetFloat("volume", out tempValue);
        mainSlider.value = tempValue;
        //sfx
        audioMixer.GetFloat("sfxvolume", out tempValue);
        sfxSlider.value = tempValue;
        //music
        audioMixer.GetFloat("musicvolume", out tempValue);
        musicSlider.value = tempValue;
    }

    private void TestVolume(string mixerType)
    {
        audioSourceForTesting = GetComponent<AudioSource>();
        if (audioSourceForTesting.isActiveAndEnabled)
        {
            if (!audioSourceForTesting.isPlaying)
            {
                audioSourceForTesting.outputAudioMixerGroup = audioMixer.FindMatchingGroups(mixerType)[0];
                if (mixerType == "Master")
                {
                    audioSourceForTesting.clip = testingNoises[0];
                }
                if (mixerType == "SfxMixer")
                {
                    audioSourceForTesting.clip = testingNoises[1];
                }
                if (mixerType == "Voice")
                {
                    audioSourceForTesting.clip = testingNoises[2];
                }
                audioSourceForTesting.Play();
            }
        }
    }
    private void ClickSound()
    {
        if (FindObjectOfType<MainMenu>() != null)
        {
            FindObjectOfType<MainMenu>().ClickSound();
        }
    }
}
