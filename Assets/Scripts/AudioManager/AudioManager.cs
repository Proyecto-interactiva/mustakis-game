using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public bool muted = false;

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found");
            return;
        }
        s.source.Play();
        Debug.Log($"Playing: {name}");
    }

    public void MuteToggle()
    {
        if (!muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;

        }
        muted = !muted;
    }

    public void SetMute(bool activate)
    {
        if (activate) { AudioListener.volume = 0; muted = true; }
        else if (!activate) { AudioListener.volume = 1; muted = false; }
    }
}
