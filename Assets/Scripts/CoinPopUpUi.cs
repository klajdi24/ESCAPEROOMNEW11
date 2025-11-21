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
        text.text = "+1 (" + newCoinCount + ")";

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

            if (vrCamera != null)
            {
                transform.LookAt(vrCamera);
                transform.Rotate(0, 180, 0);
            }

            transform.position = Vector3.Lerp(startPos, endPos, progress);

            yield return null;
        }

        Destroy(gameObject);
    }
}








