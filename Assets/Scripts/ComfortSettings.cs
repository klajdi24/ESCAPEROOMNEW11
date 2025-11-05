using UnityEngine;
using UnityEngine.UI;

public class ComfortSettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Button gearButton;
    public Slider brightnessSlider;
    public Slider motionSensitivitySlider;
    public Toggle gazeToggle;

    [Header("Scene References")]
    public Light sceneLight;                 // Your main scene light
    public GazeInteractor gazeInteractor;    // Reference to your gaze interactor script

    private bool menuOpen = false;
    private float motionMultiplier = 1f;

    void Start()
    {
        // Make sure menu starts hidden
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Hook up UI events
        if (gearButton != null)
            gearButton.onClick.AddListener(ToggleMenu);

        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.AddListener(SetBrightness);

        if (motionSensitivitySlider != null)
            motionSensitivitySlider.onValueChanged.AddListener(SetMotionSensitivity);

        if (gazeToggle != null)
            gazeToggle.onValueChanged.AddListener(SetGazeEnabled);

        // Initialize toggle state to match gaze interactor
        if (gazeInteractor != null && gazeToggle != null)
            gazeToggle.isOn = gazeInteractor.enabled;
    }

    // Toggles menu visibility
    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
        if (settingsPanel != null)
            settingsPanel.SetActive(menuOpen);
    }

    // Adjusts light intensity
    private void SetBrightness(float value)
    {
        if (sceneLight != null)
            sceneLight.intensity = Mathf.Lerp(0.5f, 2f, value);
    }

    // Adjusts (placeholder) motion sensitivity
    private void SetMotionSensitivity(float value)
    {
        motionMultiplier = Mathf.Lerp(0.5f, 2f, value);
        Debug.Log($"Motion sensitivity set to {motionMultiplier}");
        // Optional: apply to your movement system later
    }

    // Enables or disables the Gaze Interactor
    private void SetGazeEnabled(bool isEnabled)
    {
        if (gazeInteractor != null)
        {
            gazeInteractor.enabled = isEnabled;

            // 👇 Hide or show the reticle too
            if (gazeInteractor.reticle != null)
                gazeInteractor.reticle.gameObject.SetActive(isEnabled);
        }

        Debug.Log($"Gaze Interactor {(isEnabled ? "Enabled" : "Disabled")}");
    }
}







