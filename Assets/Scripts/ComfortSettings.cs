using UnityEngine;
using UnityEngine.UI;

public class ComfortSettings : MonoBehaviour
{
    [Header("UI References")]
    public Slider brightnessSlider;
    public Slider motionSensitivitySlider;

    [Header("Scene References")]
    public Light sceneLight;
    public Transform playerRig;
    public float defaultMoveSpeed = 1f;

    private float motionMultiplier = 1f;

    void Start()
    {
        brightnessSlider?.onValueChanged.AddListener(SetBrightness);
        motionSensitivitySlider?.onValueChanged.AddListener(SetMotionSensitivity);
    }

    public void SetBrightness(float value)
    {
        if (sceneLight != null)
            sceneLight.intensity = Mathf.Lerp(0.5f, 2f, value);
    }

    public void SetMotionSensitivity(float value)
    {
        motionMultiplier = Mathf.Lerp(0.5f, 2f, value);
        // if using a movement script, apply it there
    }
}

