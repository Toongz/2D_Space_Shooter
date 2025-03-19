using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip shootClip;
    public AudioClip explosionClip;
    public AudioClip warningBossClip;

    [Header("Audio Mixer")]
    public AudioMixerGroup sfxMixerGroup; 

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject); 

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup; 
    }

    public void PlayShootSound()
    {
        if (shootClip == null)
        {
            Debug.LogError("ShootClip is not assigned!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not available!");
            return;
        }

        audioSource.PlayOneShot(shootClip);
    }

    public void PlayExplosionSound()
    {
        if (explosionClip == null)
        {
            Debug.LogError("ExplosionClip is not assigned!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not available!");
            return;
        }

        audioSource.PlayOneShot(explosionClip);
    }

    public void PlayWarningBossSound()
    {
        if (warningBossClip == null)
        {
            Debug.LogError("WarningBossClip is not assigned!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not available!");
            return;
        }

        audioSource.PlayOneShot(warningBossClip);
    }
}
