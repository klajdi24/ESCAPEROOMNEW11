using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coinCount;
    public CoinPopupUI popupPrefab;
private void Awake()
{
    Instance = this;
    Debug.Log("[CoinManager] Awake on object: " + gameObject.name);
    Debug.Log("[CoinManager] popupPrefab = " + popupPrefab);
}


    public void AddCoin(Vector3 worldPosition)
    {
        coinCount++;
        Debug.Log("[CoinManager] AddCoin called. New coin count = " + coinCount);
        Debug.Log("[CoinManager] Popup spawn position = " + worldPosition);

        if (popupPrefab == null)
        {
            Debug.LogError("[CoinManager] popupPrefab is NOT assigned!");
            return;
        }

        // Spawn popup at world space position
        CoinPopupUI popup = Instantiate(popupPrefab, worldPosition, Quaternion.identity);

        if (popup == null)
        {
            Debug.LogError("[CoinManager] FAILED to instantiate popupPrefab!");
            return;
        }

        Debug.Log("[CoinManager] Popup instantiated successfully.");

        popup.Show(coinCount);
    }
}






