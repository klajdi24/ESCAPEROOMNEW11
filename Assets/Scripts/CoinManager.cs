using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public GameObject coinPopupPrefab; // assign prefab in inspector
    private int coinCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoin(Vector3 worldPos)
    {
        coinCount++;

        // Spawn popup at coin position
        GameObject popup = Instantiate(coinPopupPrefab, worldPos, Quaternion.identity);

        // Tell popup the NEW total count
        popup.GetComponent<CoinPopupUI>().ShowCount(coinCount);
    }
}



