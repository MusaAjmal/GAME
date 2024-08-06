using UnityEngine;
using System.Collections.Generic;
using static SoundPlayer;

public class SoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }
    void Start()
    {
        AudioClip BGmusic = soundDictionary["BGmusic"];

    }
    private void Update()
    {
        SoundPlayer.PlaySound("BGmusic");
    }

    public List<Sound> sounds; // List of sounds to be set in the Inspector
    private static Dictionary<string, AudioClip> soundDictionary;
    private static AudioSource audioSource;

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

    public static void PlaySound(string soundName)
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
}
