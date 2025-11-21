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

    private Transform vrCamera;

    private void Start()
    {
        vrCamera = Camera.main?.transform;

        if (vrCamera == null)
            Debug.LogError("[CoinPopupUI] VR Camera not found!");
    }

    public void Show(int newCoinCount)
    {
        // Show ONLY the coin count
        text.text = newCoinCount.ToString();
        text.color = Color.white;

        StartCoroutine(FloatUp());
    }

    private IEnumerator FloatUp()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, floatDistance, 0);

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            // Face the VR camera
            if (vrCamera != null)
            {
                transform.LookAt(vrCamera);
                transform.Rotate(0, 180, 0);
            }

            // Floating movement
            transform.position = Vector3.Lerp(startPos, endPos, progress);

            yield return null;
        }

        Destroy(gameObject);
    }
}









