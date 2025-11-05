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

        // 🪙 Hide the coin immediately to stop further gaze hits
        gameObject.SetActive(false);

        // 🔊 Play adaptive sound (safe version)
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

        // ✨ Spawn visual effect
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // 💰 Add to the coin counter
        CoinManager.Instance?.AddCoin();

        Debug.Log("Coin collected!");
    }
}




