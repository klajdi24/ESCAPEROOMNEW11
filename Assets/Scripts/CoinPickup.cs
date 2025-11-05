using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public ParticleSystem pickupEffect;
    private bool collected = false;

    public void PickUpCoin()
    {
        if (collected) return;
        collected = true;

        // 🔊 Play adaptive sound
        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // ✨ Spawn visual effect
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        Debug.Log("Coin collected!");

        // 💰 Add to the coin counter
        CoinManager.Instance?.AddCoin();

        // 🪙 Hide the coin
        gameObject.SetActive(false);
    }
}



