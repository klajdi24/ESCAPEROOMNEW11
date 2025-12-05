using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CombinationLockManager : MonoBehaviour
{
    [Header("Lock Configuration")]
    public string correctCode = "1805";
    public string currentCode = "0000";
    public float unlockDelay = 1.0f;

    [Header("Events")]
    public UnityEvent onLockOpened;

    [Header("Audio")]
    public AudioSource coinAudioSource;  // Audio plays on the coin

    private bool isLocked = true;
    private bool audioPlayed = false;

    void Start()
    {
        if (coinAudioSource == null)
        {
            Debug.LogError("CombinationLockManager: No coin AudioSource assigned!");
        }
    }

    public void UpdateTumbler(int tumblerIndex, int direction)
    {
        if (!isLocked) return;

        if (tumblerIndex >= 0 && tumblerIndex < currentCode.Length)
        {
            int currentDigit = int.Parse(currentCode[tumblerIndex].ToString());
            currentDigit = (currentDigit + direction + 10) % 10;

            char[] temp = currentCode.ToCharArray();
            temp[tumblerIndex] = (char)('0' + currentDigit);
            currentCode = new string(temp);

            CheckCode();
        }
    }

    private void CheckCode()
    {
        if (currentCode == correctCode && isLocked)
        {
            isLocked = false;

            // --- Ensure coin is active BEFORE playing audio ---
            GameObject coinObj = coinAudioSource.gameObject;
            if (!coinObj.activeInHierarchy)
            {
                coinObj.SetActive(true);
                Debug.Log("Coin activated before playing audio.");
            }

            // --- Play audio ---
            if (!audioPlayed && coinAudioSource != null)
            {
                coinAudioSource.Play();
                audioPlayed = true;
                Debug.Log("Correct Code Entered. Playing coin audio...");
            }

            StartCoroutine(UnlockSequenceAfterDelay());
        }
    }

    private IEnumerator UnlockSequenceAfterDelay()
    {
        yield return new WaitForSeconds(unlockDelay);
        onLockOpened.Invoke();
        Debug.Log("Lock Opened Event Invoked.");
    }

    public void ResetLock()
    {
        isLocked = true;
        currentCode = "0000";
        audioPlayed = false;
        Debug.Log("Lock state reset.");
    }
}



