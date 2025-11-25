using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SubtitledAudioLog : MonoBehaviour
{
    [Header("Content")]
    [Tooltip("The audio clip to play")]
    public AudioClip logAudio;

    [Tooltip("The text to display as subtitles")]
    [TextArea(3, 5)]
    public string logText;

    [Header("References")]
    [Tooltip("The TextMeshPro UI element to display the subtitles on")]
    public TMP_Text subtitleDisplay;

    [Tooltip("How long to show the subtitles after the audio finishes")]
    public float subtitleLingerTime = 2.0f;

    private AudioSource audioSource;
    private bool hasBeenPlayed = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = logAudio;
        audioSource.playOnAwake = false;

        if (subtitleDisplay == null)
            Debug.LogError("[SubtitledAudioLog] Subtitle Display (TMP_Text) is not assigned!");
        else
            subtitleDisplay.gameObject.SetActive(false); // Hide text initially
    }

    // This is the function you will call from the GazeInteractable's onActivate event
    public void PlayLog()
    {
        // Prevent re-playing spam
        if (hasBeenPlayed || (audioSource != null && audioSource.isPlaying))
            return;

        if (logAudio == null || subtitleDisplay == null)
            return;

        StartCoroutine(PlayAndShowSubtitles());
    }

    private IEnumerator PlayAndShowSubtitles()
    {
        hasBeenPlayed = true;

        // Show text and play audio
        subtitleDisplay.text = logText;
        subtitleDisplay.gameObject.SetActive(true);
        audioSource.Play();

        // Wait for the audio clip to finish
        yield return new WaitForSeconds(logAudio.length);

        // Wait for the "linger time"
        yield return new WaitForSeconds(subtitleLingerTime);

        // Hide the subtitles
        subtitleDisplay.gameObject.SetActive(false);
        hasBeenPlayed = false; // Allow re-playing
    }
}
