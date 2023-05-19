using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }    

    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
        StarForce,
        Success,
        Failed,
        Destroy,
    }

    private AudioSource audioSource;

    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    private float volume = .5f;

    private void Awake()
    {
        volume = PlayerPrefs.GetFloat("soundVolume", .5f);
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach(Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}