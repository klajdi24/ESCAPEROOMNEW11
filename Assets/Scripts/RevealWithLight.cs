using UnityEngine;
using System.Collections;

public class RevealWithLight : MonoBehaviour
{
    [Header("Child Object Settings")]
    public GameObject coinChild;          

    [Header("Light Settings")]
    public Light targetLight;            
    public float revealThreshold = 53f;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;     

    private Renderer coinRenderer;
    private bool revealed = false;
    private Color originalColor;

    void Start()
    {
        if (coinChild == null)
        {
            Debug.LogError("Coin child not assigned!");
            return;
        }

        coinRenderer = coinChild.GetComponent<Renderer>();
        if (coinRenderer == null)
        {
            Debug.LogError("Coin child has no Renderer!");
            return;
        }

        originalColor = coinRenderer.material.color;

        
        Color c = originalColor;
        c.a = 0f;
        coinRenderer.material.color = c;

        
        coinChild.SetActive(false);
    }

    void Update()
    {
        if (targetLight == null || coinChild == null) return;

        if (targetLight.intensity >= revealThreshold && !revealed)
        {
            revealed = true;
            coinChild.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color c = coinRenderer.material.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, originalColor.a, timer / fadeDuration);
            c.a = alpha;
            coinRenderer.material.color = c;
            yield return null;
        }

        
        c.a = originalColor.a;
        coinRenderer.material.color = c;
    }
}




