using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    bool collected = false;

    public void PickUpCoin()
    {
        if (collected) return;
        collected = true;

        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Debug.Log("Coin collected!");
        gameObject.SetActive(false);
    }
}

