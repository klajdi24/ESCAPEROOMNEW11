using UnityEngine;
using UnityEngine.UI;

public class ComfortSettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Button gearButton;
    public Button closeButton;
    public Slider brightnessSlider;
    public Slider motionSensitivitySlider;
    public Toggle gazeToggle;

    [Header("Scene References")]
    public Light sceneLight;
    public GazeInteractor gazeInteractor;

    private bool menuOpen = false;
    private float motionMultiplier = 1f;

    // Brightness limits
    [SerializeField] private float minBrightness = 0.2f;
    [SerializeField] private float maxBrightness = 3f;

    private void Start()
    {
        // Hide menu at start
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Hook buttons
        if (gearButton != null)
            gearButton.onClick.AddListener(ToggleMenu);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        // Brightness setup
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value = 0.5f;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            SetBrightness(brightnessSlider.value);
        }

        // Motion
        if (motionSensitivitySlider != null)
            motionSensitivitySlider.onValueChanged.AddListener(SetMotionSensitivity);

        // Gaze toggle
        if (gazeToggle != null)
            gazeToggle.onValueChanged.AddListener(SetGazeEnabled);

        if (gazeInteractor != null && gazeToggle != null)
            gazeToggle.isOn = gazeInteractor.enabled;
    }

    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
        if (settingsPanel != null)
            settingsPanel.SetActive(menuOpen);
    }

    private void CloseMenu()
    {
        menuOpen = false;
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void SetBrightness(float value)
    {
        if (sceneLight == null)
        {
            Debug.LogWarning("⚠️ No Light assigned to ComfortSettingsMenu!");
            return;
        }

        float newIntensity = Mathf.Lerp(minBrightness, maxBrightness, value);
        sceneLight.intensity = newIntensity;
        Debug.Log($"💡 Brightness Slider: {value:F2} → Light Intensity: {newIntensity:F2}");
    }

    private void SetMotionSensitivity(float value)
    {
        motionMultiplier = Mathf.Lerp(0.5f, 2f, value);
        Debug.Log($"Motion sensitivity set to {motionMultiplier}");
    }

    private void SetGazeEnabled(bool isEnabled)
    {
        if (gazeInteractor != null)
        {
            gazeInteractor.enabled = isEnabled;

            if (gazeInteractor.reticle != null)
                gazeInteractor.reticle.gameObject.SetActive(isEnabled);
        }

        Debug.Log($"Gaze Interactor {(isEnabled ? "Enabled" : "Disabled")}");
    }
}










