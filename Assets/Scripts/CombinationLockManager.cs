using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CombinationLockManager : MonoBehaviour
{
    
    [Header("Subtitles")]
    public VRSubtitleManager subtitleManager;
    public AudioClip captainVoiceClip;
    public Sprite captainIcon;

    [TextArea]
    public string[] subtitleLines; 

    
    [Header("Lock Configuration")]
    public string correctCode = "1805";
    public string currentCode = "0000";
    public float unlockDelay = 1.0f;

    
    [Header("Events")]
    public UnityEvent onLockOpened;

    
    [Header("Audio")]
    public AudioSource coinAudioSource; 

   
    private bool isLocked = true;
    private bool audioPlayed = false;

    void Start()
    {
        if (coinAudioSource == null)
            Debug.LogError("CombinationLockManager: No coin AudioSource assigned!");

        if (subtitleManager == null)
            Debug.LogWarning("CombinationLockManager: No Subtitle Manager assigned!");
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

            
            GameObject coinObj = coinAudioSource.gameObject;
            if (!coinObj.activeInHierarchy)
            {
                coinObj.SetActive(true);
                Debug.Log("Coin object activated.");
            }

            
            if (!audioPlayed && coinAudioSource != null)
            {
                coinAudioSource.clip = captainVoiceClip;
                coinAudioSource.Play();
                audioPlayed = true;
                Debug.Log("Correct code entered. Coin voice playing...");

               
                string[] captainLines = new string[]
                {
                    "Well, blow me down.",
                    "Look at the little landlubber. You actually got the lock open.",
                    "Figured you'd be stuck on that for days.",
                    "I really did.",
                    "Don't worry.",
                    "I won't tell the crew you only beat the first puzzle.",
                    "because I accidentally left the code as 1805.",
                    "Alright, listen up, bilge rat.",
                    "The next secret ain't hidden in a chest.",
                    "It's staring you right in the face.",
                    "But your thick skull keeps missing the shine.", 
                    "You gotta look closer. It's a bright clue, matey.",
                    "Now stop scratching your head like a monkey."
                };

                if (subtitleManager != null)
                {
                    
                    subtitleManager.ShowSubtitles(captainLines, captainIcon);
                }
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










