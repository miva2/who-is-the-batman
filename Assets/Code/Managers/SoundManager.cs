using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField]
    private FxLibrary fxLibrary;

    [SerializeField] private AudioClip music;
    
    private Dictionary<string, AudioSource> sources;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        sources = new Dictionary<string, AudioSource>();

        foreach (var fx in fxLibrary.fx)
        {
            if (fx.sound == null || string.IsNullOrEmpty(fx.key))
                continue;

            if (sources.ContainsKey(fx.key))
            {
                Debug.LogError($"Duplicate FX key: {fx.key}");
                continue;
            }

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = fx.sound;
            source.loop = false;
            source.volume = 1f;
            source.playOnAwake = false;

            sources.Add(fx.key, source);
        }
        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.volume = 1f;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayFX(string key)
    {
        if (sources.TryGetValue(key, out var source))
        {
            source.Stop();          
            source.Play();
        }
        else
        {
            Debug.LogError($"FX not found: {key}");
        }
    }
}
