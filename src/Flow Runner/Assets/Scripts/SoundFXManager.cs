using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip auidoClip, Transform spawnTransform)
    {
        // spawn in game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform);

        // assign audioClip
        audioSource.clip = auidoClip;

        // assign volume
        audioSource.volume = SettingsManager.Instance.Volume;

        // play sound
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
