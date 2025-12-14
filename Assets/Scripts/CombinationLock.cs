using UnityEngine;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    [Header("Solution")]
    [Tooltip("The correct code, e.g., '1805'")]
    public string correctCode = "1805";

    [Header("References")]
    [Tooltip("Assign all 4 of your GazeLockTumbler scripts here, in order (left to right)")]
    public GazeLockTumbler[] tumblers;

    [Header("Events")]
    public UnityEvent onSolved;
    public UnityEvent onFailedAttempt;

    public AudioClip successSound;
    public AudioClip failSound;
    private AudioSource audioSource;
    private bool isSolved = false;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
    }

    
    public void CheckCode()
    {
        if (isSolved) return; 

        string currentCode = "";
        foreach (GazeLockTumbler tumbler in tumblers)
        {
            currentCode += tumbler.GetValue().ToString();
        }

        if (currentCode == correctCode)
        {
            Solve();
        }
        else
        {
            
            if (failSound != null)
                audioSource.PlayOneShot(failSound);
            
            onFailedAttempt?.Invoke();
        }
    }

    private void Solve()
    {
        isSolved = true;
        Debug.Log("Lock Solved!");

        if (successSound != null)
            audioSource.PlayOneShot(successSound);

        
        onSolved?.Invoke();
    }
}
