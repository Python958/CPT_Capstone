using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitcherScript : MonoBehaviour
{
    public AudioSource[] aSources;

    private AudioListener aListener;

    // Start is called before the first frame update
    void Start()
    {
        aListener = gameObject.GetComponent<AudioListener>();
        if(aListener == null) { Debug.Log("can't find Audio Listener"); }
    }

    public void PlaySound(AudioClip sound)
    {
        bool success = false;
        for (var i = 0; i < aSources.Length; i++)
        {
            if (!aSources[i].isPlaying)
            {
                success = true;
                aSources[i].clip = sound;
                aSources[i].loop = false;
                aSources[i].Play();
                break;
            }
        }
        if (!success) { Debug.Log("no unused audio sources right now"); }
    }//play without looping

    public void PlaySound(AudioClip sound, bool shouldLoop)
    {
        bool success = false;
        for (var i = 0; i < aSources.Length; i++)
        {
            if (!aSources[i].isPlaying)
            {
                success = true;
                aSources[i].clip = sound;
                aSources[i].loop = shouldLoop;
                aSources[i].Play();
                break;
            }
        }
        if (!success) { Debug.Log("no unused audio sources right now"); }
    }//play with looping

    public void KillSound(AudioClip sound)
    {
        //bool success = false;
        for (var i = 0; i < aSources.Length; i++)
        {
            if (aSources[i].isPlaying && aSources[i].clip == sound)
            {
                //success = true;
                aSources[i].Stop();
            }
        }
        //if (!success) { Debug.Log("that sound is not playing now"); }
    }//kill a sound (especially useful if you have a looping sound that you want to stop)

    public bool CheckIfSoundIsPlaying(AudioClip clip)
    {
        for(int i = 0; i < aSources.Length; i++)
        {
            if(aSources[i].isPlaying)
            {
                if (aSources[i].clip == clip)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
