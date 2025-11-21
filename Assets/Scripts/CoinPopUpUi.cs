using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CoinPopupUI : MonoBehaviour
{
    public TMP_Text text;
    public Image icon;

    public float floatDistance = 0.2f;
    public float duration = 1f;

    private CanvasGroup group;
    private Transform vrCamera;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();

        if (group == null)
        {
            Debug.LogWarning("[CoinPopupUI] CanvasGroup missing. Adding one.");
            group = gameObject.AddComponent<CanvasGroup>();
        }

        vrCamera = Camera.main != null ? Camera.main.transform : null;

        if (vrCamera == null)
            Debug.LogError("[CoinPopupUI] VR Camera NOT FOUND! (Camera.main was null)");

        Debug.Log("[CoinPopupUI] Awake complete.");
    }

    public void Show(int newCoinCount)
    {
        Debug.Log("[CoinPopupUI] Show() called. New coin count = " + newCoinCount);

        if (text == null)
            Debug.LogError("[CoinPopupUI] Text reference NOT assigned!");

        text.text = "+1  (" + newCoinCount + ")";

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Debug.Log("[CoinPopupUI] Animate() coroutine started.");

        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * floatDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = t / duration;

            // Always face camera in VR
            if (vrCamera != null)
            {
                transform.LookAt(vrCamera);
                transform.Rotate(0, 180, 0);
            }
            else
            {
                Debug.LogWarning("[CoinPopupUI] No VR camera found during animation.");
            }

            // Move upward
            transform.position = Vector3.Lerp(start, end, n);

            // Fade out
            group.alpha = 1 - n;

            yield return null;
        }

        Debug.Log("[CoinPopupUI] Animation finished. Destroying popup.");

        Destroy(gameObject);
    }
}




