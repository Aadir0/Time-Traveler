using UnityEngine;

public class SoundManagerSimple : MonoBehaviour
{
    private AudioSource Source;

    private void Awake()
    {
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
