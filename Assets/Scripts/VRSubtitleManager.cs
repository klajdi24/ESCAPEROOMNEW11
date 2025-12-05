using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VRSubtitleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI subtitleText;
    public Image characterIcon;

    [Header("Timing")]
    public float charactersPerSecond = 20f; // typing speed
    public float extraHoldTime = 1.0f;      // extra time after audio ends

    [Header("VR")]
    public Transform playerCamera; // assign XR camera

    private Coroutine subtitleRoutine;

    void Update()
    {
        // Make subtitles always face the player
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180, 0); 
        }
    }

    public void ShowSubtitles(string subtitle, AudioClip audio, Sprite speakerIcon = null)
    {
        if (subtitleRoutine != null)
            StopCoroutine(subtitleRoutine);

        subtitleRoutine = StartCoroutine(SubtitleSequence(subtitle, audio, speakerIcon));
    }

    private IEnumerator SubtitleSequence(string subtitle, AudioClip audio, Sprite speakerIcon)
    {
        // Set speaker icon
        if (characterIcon != null)
            characterIcon.sprite = speakerIcon;

        subtitleText.text = "";
        float typingTime = subtitle.Length / charactersPerSecond;

        float displayDuration = audio.length + extraHoldTime;

        // TYPE OUT THE SUBTITLES
        float timer = 0f;
        while (timer < typingTime)
        {
            timer += Time.deltaTime;
            int charsToShow = Mathf.FloorToInt((timer / typingTime) * subtitle.Length);
            subtitleText.text = subtitle.Substring(0, charsToShow);
            yield return null;
        }

        subtitleText.text = subtitle;

        yield return new WaitForSeconds(displayDuration);

        subtitleText.text = "";
    }
}

