using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] soundClips;
    public Sound[] musicClips;

    [Range(0.1f, 1f)]
    public float soundVolume = 1f;

    [Range(0.1f, 1f)]
    public float musicVolume = 0.5f;

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

        foreach(Sound s in musicClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = musicVolume;
            s.source.loop = s.loop;
            s.source.spatialBlend = spatialBlend;
        }
    }

    public void Start()
    {
        Sound s = Array.Find(musicClips, sound => sound.name == "LevelTheme");

        s.source.Play();
    }

    public void PlayClip(string name)
    {
        Sound s = Array.Find(soundClips, sound => sound.name == name);

        s.source.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        var s = Array.Find(soundClips, sound => sound.clip == clip);
        s.source.Play();
    }

    public async UniTask PlayClipAsync(string name)
    {
        Sound s = Array.Find(soundClips, sound => sound.name == name);
        s.source.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(s.clip.length), cancellationToken: this.GetCancellationTokenOnDestroy(), ignoreTimeScale: true);
    }
}
