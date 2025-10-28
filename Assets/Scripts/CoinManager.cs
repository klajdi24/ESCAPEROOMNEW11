using UnityEngine;
using TMPro; // needed for TMP text
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; // allows access from other scripts

    public TextMeshProUGUI coinText;    // drag your CoinText here
    public Image coinIcon;              // drag your CoinIcon here (optional)
    private int coinCount = 0;          // total coins collected

    void Awake()
    {
        // make sure only one CoinManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "x " + coinCount;
    }
}

