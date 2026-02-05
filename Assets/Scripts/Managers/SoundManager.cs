using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set;}
    private AudioSource Source;

    private void Awake()
    {
        // Keep this object when loading new scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy duplicate game objects
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Source = GetComponent<AudioSource>();
    }

    public void Playsound(AudioClip _sound)
    {
        Source.PlayOneShot(_sound);
    }

    public void StopAll()
    {
        Source.Stop();
    }
}