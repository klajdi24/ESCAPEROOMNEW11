using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource; // Background music
    [HideInInspector] public float musicVolume = 1f;

    [Header("SFX Settings")]
    [Range(0,1)] public float sfxVolume = 1f;

    // Singleton for easy access
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateMusicVolume();
    }

    // Call this from the Music slider
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        UpdateMusicVolume();
    }

    void UpdateMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    // Call this when playing SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, sfxVolume);
    }

    // Call this from the SFX slider
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }
}

