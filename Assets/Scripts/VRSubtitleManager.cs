using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VRSubtitleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI subtitleText;   // Assign in Canvas
    public Image characterIcon;            // Optional: assign Image for character icon

    [Header("VR Settings")]
    public Transform playerCamera;         // XR Camera

    [Header("Timing")]
    [Tooltip("Number of words displayed per second. Adjust to sync subtitle pacing with audio.")]
    public float wordsPerSecond = 2f;      // Inspector-adjustable

    public float extraHoldTime = 0.3f;     // Extra pause after each sentence

    private Coroutine subtitleRoutine;

    void Update()
    {
        // Keep subtitles facing the player
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180f, 0);  // Flip correctly
        }
    }

    /// <summary>
    /// Show subtitles one sentence at a time.
    /// </summary>
    public void ShowSubtitles(string[] sentences, Sprite speakerIcon = null)
    {
        if (subtitleRoutine != null)
            StopCoroutine(subtitleRoutine);

        subtitleRoutine = StartCoroutine(SubtitleSequence(sentences, speakerIcon));
    }

    private IEnumerator SubtitleSequence(string[] sentences, Sprite speakerIcon)
    {
        if (characterIcon != null && speakerIcon != null)
            characterIcon.sprite = speakerIcon;

        foreach (string sentence in sentences)
        {
            subtitleText.text = ""; // Clear previous sentence

            string[] words = sentence.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                subtitleText.text += words[i] + " ";

                // Wait based on Inspector-controlled wordsPerSecond
                yield return new WaitForSeconds(1f / wordsPerSecond);
            }

            // Hold sentence for readability
            yield return new WaitForSeconds(extraHoldTime);

            // Clear for next sentence
            subtitleText.text = "";
        }
    }
}









