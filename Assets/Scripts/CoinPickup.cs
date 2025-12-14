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

        gameObject.SetActive(false);

        
        if (pickupSound && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(pickupSound, transform.position);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(transform.position);
        }

        Debug.Log("Coin collected!");
    }
}








