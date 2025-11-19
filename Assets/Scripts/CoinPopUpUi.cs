using UnityEngine;
using TMPro;

public class CoinPopupUI : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public float riseHeight = 1f;
    public float riseSpeed = 1f;
    public float fadeDuration = 0.5f;

    CanvasGroup cg;
    Vector3 startPos;
    Vector3 endPos;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        startPos = transform.position;
        endPos = startPos + Vector3.up * riseHeight;
    }

    public void ShowCount(int count)
    {
        countText.text = count.ToString();
        StartCoroutine(Animate());
    }

    private System.Collections.IEnumerator Animate()
    {
        float t = 0f;

        // rise animation
        while (t < 1f)
        {
            t += Time.deltaTime * riseSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // fade out
        float ft = 0f;
        while (ft < fadeDuration)
        {
            ft += Time.deltaTime;
            cg.alpha = 1f - (ft / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}

