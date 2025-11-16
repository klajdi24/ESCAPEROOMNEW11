using UnityEngine;
using TMPro; // Assuming you are using TextMeshPro for modern UI
using System.Collections;

public class CoinManager : MonoBehaviour
{
    // --- SINGLETON SETUP (Required for CoinManager.Instance) ---
    public static CoinManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional: keep coin count across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize coin count on awake
        UpdateCoinCountUI();
        
        // Ensure the animation UI is hidden initially
        if (coinIconGroup != null)
            coinIconGroup.localScale = Vector3.zero;
    }
    // -----------------------------------------------------------

    // --- UI REFERENCES ---
    [Header("Coin UI References")]
    public TextMeshProUGUI coinCountText; // The main text showing the total
    public Transform coinIconGroup;       // The parent of the coin icon/text to animate (e.g., a small Panel)
    public TextMeshProUGUI animationText; // Optional: A separate text to show "+1" or the new count temporarily

    private int coinCount = 0;
    private Coroutine animationRoutine;
    
    // --- CONFIG ---
    [Header("Animation Settings")]
    public float popUpDuration = 0.5f;   // How long the animation lasts
    public float popUpScale = 1.3f;      // How large the icon gets (1.3x normal)
    public float returnDelay = 0.2f;     // Delay before shrinking/fading out

    // --- COIN LOGIC ---
    public void AddCoin()
    {
        coinCount++;
        UpdateCoinCountUI();
        ShowCoinAnimation(); // <--- New call to trigger the visual feedback
    }

    private void UpdateCoinCountUI()
    {
        if (coinCountText != null)
        {
            coinCountText.text = coinCount.ToString();
        }
    }

    // --- ANIMATION LOGIC ---
    private void ShowCoinAnimation()
    {
        // Stop any existing routine to prevent overlap
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        // Update the text immediately
        if (animationText != null)
        {
             // Option 1: Show "+1" or just the new count
             animationText.text = "+" + 1; // Or coinCount.ToString();
        }

        animationRoutine = StartCoroutine(AnimateCoinUI());
    }

    private IEnumerator AnimateCoinUI()
    {
        if (coinIconGroup == null) yield break;

        // 1. POP UP (Fast Scale Up)
        float timer = 0f;
        Vector3 startScale = coinIconGroup.localScale;
        Vector3 peakScale = Vector3.one * popUpScale;

        while (timer < popUpDuration / 2f)
        {
            timer += Time.deltaTime;
            float t = timer / (popUpDuration / 2f);
            coinIconGroup.localScale = Vector3.Lerp(startScale, peakScale, t);
            yield return null;
        }

        // 2. PAUSE
        yield return new WaitForSeconds(returnDelay);

        // 3. SHRINK AND FADE OUT (Fast Scale Down)
        timer = 0f;
        startScale = coinIconGroup.localScale;
        Vector3 endScale = Vector3.one; // Return to normal size (or Vector3.zero to hide)

        while (timer < popUpDuration / 2f)
        {
            timer += Time.deltaTime;
            float t = timer / (popUpDuration / 2f);
            coinIconGroup.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // Ensure it ends at the target scale
        coinIconGroup.localScale = endScale;
        animationRoutine = null;
    }
}

