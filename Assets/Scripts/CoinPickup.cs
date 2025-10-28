using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    bool collected = false;

    public void PickUpCoin()
    {
        if (collected) return;
        collected = true;

        // 🔊 Play sound if assigned
        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Debug.Log("Coin collected!");

        // 💰 Add to the coin counter (NEW)
        if (CoinManager.Instance != null)
            CoinManager.Instance.AddCoin();

        // 🪙 Hide or disable this coin
        gameObject.SetActive(false);
    }
}


