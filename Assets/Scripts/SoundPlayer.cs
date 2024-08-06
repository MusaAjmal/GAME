using UnityEngine;
using System.Collections.Generic;

public class SoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds; // List of sounds to be set in the Inspector

    private static Dictionary<string, AudioClip> soundDictionary;
    private static AudioSource audioSource;

    // Volume control fields
    private static float minVolume = 0f;  // Minimum volume
    private static float maxVolume = 1f;  // Maximum volume
    private static float volumeStep = 0.1f;  // Step for volume increase/decrease

    void Awake()
    {
        // Initialize the dictionary and audio source
        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (Sound sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary[sound.name] = sound.clip;
            }
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // Play background music once
        PlaySound("BGmusic", true);
    }

    // Method to play sound with optional looping
    public static void PlaySound(string soundName, bool loop = false)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.clip = soundDictionary[soundName];
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("SoundPlayer: Sound not found - " + soundName);
        }
    }

    // Method to play a one-shot sound
    public static void PlayOneShotSound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning("SoundPlayer: Sound not found - " + soundName);
        }
    }

    // Method to stop currently playing sound
    public static void StopSound()
    {
        audioSource.Stop();
    }

    // Method to set volume (absolute)
    public static void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }

    // Method to increase volume
    public static void IncreaseVolume()
    {
        float newVolume = Mathf.Clamp(audioSource.volume + volumeStep, minVolume, maxVolume);
        audioSource.volume = newVolume;
    }

    // Method to decrease volume
    public static void DecreaseVolume()
    {
        float newVolume = Mathf.Clamp(audioSource.volume - volumeStep, minVolume, maxVolume);
        audioSource.volume = newVolume;
    }
}
