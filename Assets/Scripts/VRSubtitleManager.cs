using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class VRSubtitleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject subtitlePanel;        // The panel object you disable/enable
    public TextMeshProUGUI subtitleText;
    public Image characterIcon;

    [Header("VR Settings")]
    public Transform playerCamera;

    [Header("Timing")]
    public float wordsPerSecond = 3f;
    public float extraHoldTime = 0.3f;

    private Coroutine subtitleRoutine;

    void Start()
    {
        // Make sure panel is hidden at start
        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);
    }

    void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180f, 0);
        }
    }

    public void ShowSubtitles(string[] sentences, Sprite speakerIcon = null)
    {
        if (subtitleRoutine != null)
            StopCoroutine(subtitleRoutine);

        subtitleRoutine = StartCoroutine(SubtitleSequence(sentences, speakerIcon));
    }

    private IEnumerator SubtitleSequence(string[] sentences, Sprite speakerIcon)
    {
        // Apply speaker icon
        if (characterIcon != null && speakerIcon != null)
            characterIcon.sprite = speakerIcon;

        // --- IMPORTANT ---
        // Activate ONLY the subtitle panel, NOT the entire Canvas
        subtitlePanel.SetActive(true);

        foreach (string sentence in sentences)
        {
            subtitleText.text = "";
            string[] words = sentence.Split(' ');

            foreach (string w in words)
            {
                subtitleText.text += w + " ";
                yield return new WaitForSeconds(1f / wordsPerSecond);
            }

            yield return new WaitForSeconds(extraHoldTime);
            subtitleText.text = "";
        }

        // Hide afterwards
        subtitlePanel.SetActive(false);
    }
}













