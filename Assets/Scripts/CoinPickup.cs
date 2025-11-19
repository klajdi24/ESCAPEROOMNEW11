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

        // Hide coin immediately (prevents double pickup)
        gameObject.SetActive(false);

        // Play adaptive sound
        if (pickupSound)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;

            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.clip = pickupSound;
            source.spatialBlend = 1f;
            source.Play();

            Destroy(tempAudio, pickupSound.length);
        }

        // Spawn particle effect
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // Add to the coin counter + spawn popup
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(transform.position);
        }
        else
        {
            Debug.LogError("No CoinManager found in the scene!");
        }

        Debug.Log("Coin collected!");
    }
}






