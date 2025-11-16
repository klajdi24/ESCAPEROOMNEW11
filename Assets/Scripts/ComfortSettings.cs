using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Canvas))]
public class ComfortSettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Button closeButton;
    public Slider brightnessSlider;
    public Slider motionSensitivitySlider;
    public Toggle gazeToggle;

    [Header("Scene References")]
    public Light sceneLight;
    public MonoBehaviour gazeInteractor;

    [Header("XR Locomotion")]
    [Tooltip("Drag the Move Provider component (ContinuousMoveProviderBase, DeviceBased, ActionBased, etc.)")]
    public Component moveProvider;

    [Header("Placement")]
    public Camera playerCamera;
    public float openDistance = 1.5f;
    public float verticalOffset = -0.2f;

    private bool menuOpen = false;
    private float baseMoveSpeed = 1f;

    private InputAction toggleAction;

    private PropertyInfo moveSpeedProperty;
    private FieldInfo moveSpeedField;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        toggleAction = new InputAction(
            name: "ToggleMenu",
            type: InputActionType.Button,
            binding: "<XRController>{LeftHand}/primaryButton"
        );
        toggleAction.Enable();
    }

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value = 0.5f;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            SetBrightness(brightnessSlider.value);
        }

        if (motionSensitivitySlider != null)
        {
            motionSensitivitySlider.minValue = 0f;
            motionSensitivitySlider.maxValue = 1f;
            motionSensitivitySlider.value = 0.5f;
            motionSensitivitySlider.onValueChanged.AddListener(SetMotionSensitivity);
        }

        if (gazeToggle != null)
            gazeToggle.onValueChanged.AddListener(SetGazeEnabled);

        if (gazeInteractor != null && gazeToggle != null)
            gazeToggle.isOn = gazeInteractor.enabled;

        if (moveProvider != null)
        {
            Type t = moveProvider.GetType();

            moveSpeedProperty = t.GetProperty("moveSpeed",
                BindingFlags.Public | BindingFlags.Instance);

            if (moveSpeedProperty == null)
            {
                moveSpeedField = t.GetField("moveSpeed",
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            }

            try
            {
                if (moveSpeedProperty != null)
                    baseMoveSpeed = (float)moveSpeedProperty.GetValue(moveProvider);
                else if (moveSpeedField != null)
                    baseMoveSpeed = (float)moveSpeedField.GetValue(moveProvider);
                else
                {
                    var propAlt = t.GetProperty("m_MoveSpeed",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    if (propAlt != null)
                        baseMoveSpeed = (float)propAlt.GetValue(moveProvider);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("ComfortSettingsMenu: Could not read moveSpeed: " + ex.Message);
            }
        }
    }

    private void Update()
    {
        if (toggleAction != null && toggleAction.WasPressedThisFrame())
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        menuOpen = !menuOpen;

        if (settingsPanel == null)
        {
            Debug.LogWarning("ComfortSettingsMenu: settingsPanel not assigned.");
            return;
        }

        if (menuOpen)
        {
            PlacePanelInFrontOfPlayer();
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }
    }

    private void PlacePanelInFrontOfPlayer()
    {
        if (playerCamera == null)
            return;

        Vector3 target = playerCamera.transform.position
                         + playerCamera.transform.forward.normalized * openDistance
                         + Vector3.up * verticalOffset;

        settingsPanel.transform.position = target;

        Vector3 faceDirection = playerCamera.transform.forward;
        faceDirection.y = 0f;

        settingsPanel.transform.rotation = Quaternion.LookRotation(faceDirection);
    }

    public void CloseMenu()
    {
        menuOpen = false;
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void SetBrightness(float value)
    {
        if (sceneLight == null) return;

        float newIntensity = Mathf.Lerp(0.2f, 3f, value);
        sceneLight.intensity = newIntensity;
    }

    private void SetMotionSensitivity(float value)
    {
        if (moveProvider == null) return;

        float multiplier = Mathf.Lerp(0.5f, 2f, value);
        float newSpeed = baseMoveSpeed * multiplier;

        try
        {
            if (moveSpeedProperty != null)
            {
                moveSpeedProperty.SetValue(moveProvider, newSpeed);
                return;
            }

            if (moveSpeedField != null)
            {
                moveSpeedField.SetValue(moveProvider, newSpeed);
                return;
            }

            var propAlt = moveProvider.GetType().GetProperty("m_MoveSpeed",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (propAlt != null)
            {
                propAlt.SetValue(moveProvider, newSpeed);
                return;
            }

            Debug.LogWarning("ComfortSettingsMenu: moveSpeed not found, cannot modify speed.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("ComfortSettingsMenu: Could not set moveSpeed: " + ex.Message);
        }
    }

    private void SetGazeEnabled(bool isEnabled)
    {
        if (gazeInteractor == null) return;

        gazeInteractor.enabled = isEnabled;

        var t = gazeInteractor.GetType();
        var reticleProp = t.GetProperty("reticle",
            BindingFlags.Public | BindingFlags.Instance);

        if (reticleProp != null)
        {
            Component retObj = reticleProp.GetValue(gazeInteractor) as Component;
            if (retObj != null)
                retObj.gameObject.SetActive(isEnabled);
        }
        else
        {
            var reticleField = t.GetField("reticle",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            if (reticleField != null)
            {
                Component retObj = reticleField.GetValue(gazeInteractor) as Component;
                if (retObj != null)
                    retObj.gameObject.SetActive(isEnabled);
            }
        }
    }
}
