using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] soundClips;

    [Range(0.1f, 1f)]
    public float soundVolume = 1f;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in soundClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = soundVolume;
            s.source.loop = s.loop;
            s.source.spatialBlend = spatialBlend;
        }
    }

    public void PlayClip(string name)
    {
        Sound s = Array.Find(soundClips, sound => sound.name == name);

        s.source.Play();
    }
}
