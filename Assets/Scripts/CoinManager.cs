using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public GameObject popupPrefab;
    private int coinCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (popupPrefab == null)
            Debug.LogError("[CoinManager] popupPrefab NOT assigned!");
        else
            Debug.Log("[CoinManager] popupPrefab assigned.");
    }

    public void AddCoin(Vector3 worldPos)
    {
        coinCount++;

        if (popupPrefab == null)
        {
            Debug.LogError("[CoinManager] popupPrefab is NULL");
            return;
        }

        // This is the earlier behavior
        GameObject popup = Instantiate(popupPrefab, worldPos, Quaternion.identity);

        var ui = popup.GetComponent<CoinPopupUI>();
        if (ui != null)
            ui.Show(coinCount);
    }
}








