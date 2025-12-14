using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;
    [Range(0, 1)] public float musicVolume = 1f;

    [Header("SFX")]
    [Range(0, 1)] public float sfxVolume = 1f;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ApplyMusicVolume();
    }

    
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyMusicVolume();
    }

    void ApplyMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

   
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempSFX");
        temp.transform.position = position;

        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = sfxVolume;
        source.spatialBlend = 1f; 
        source.Play();

        Destroy(temp, clip.length);
    }
}


