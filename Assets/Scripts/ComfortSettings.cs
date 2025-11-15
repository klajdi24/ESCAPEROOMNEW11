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
    public MonoBehaviour gazeInteractor; // keep generic (GazeInteractor or similar)

    [Header("XR Locomotion")]
    [Tooltip("Drag the Move Provider component (click component header and drag) - not the GameObject")]
    public Component moveProvider; // intentionally generic: Action-based, Device-based, ContinuousMoveProviderBase, etc.

    [Header("Placement")]
    public Camera playerCamera;
    public float openDistance = 1.5f;
    public float verticalOffset = -0.2f;

    private bool menuOpen = false;
    private float baseMoveSpeed = 1f;

    // Runtime InputAction (no InputActions asset required)
    private InputAction toggleAction;

    // Reflection helpers cached
    private PropertyInfo moveSpeedProperty;
    private FieldInfo moveSpeedField;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Create automatic toggle input (no asset required).
        // Binds to left controller primary button (X on Quest left controller).
        toggleAction = new InputAction(
            name: "ToggleMenu",
            type: InputActionType.Button,
            binding: "<XRController>{LeftHand}/primaryButton"
        );
        toggleAction.Enable();
    }

    private void Start()
    {
        // UI wiring (safe checks)
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
        {
            // try to read an "enabled" state if possible
            gazeToggle.isOn = gazeInteractor.enabled;
        }

        // If a moveProvider is assigned, try to read its current moveSpeed via reflection
        if (moveProvider != null)
        {
            Type t = moveProvider.GetType();
            // common property name: "moveSpeed"
            moveSpeedProperty = t.GetProperty("moveSpeed", BindingFlags.Public | BindingFlags.Instance);
            if (moveSpeedProperty == null)
            {
                // some older types may use a field
                moveSpeedField = t.GetField("moveSpeed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            }

            try
            {
                if (moveSpeedProperty != null && moveSpeedProperty.PropertyType == typeof(float))
                {
                    baseMoveSpeed = (float)moveSpeedProperty.GetValue(moveProvider);
                }
                else if (moveSpeedField != null && moveSpeedField.FieldType == typeof(float))
                {
                    baseMoveSpeed = (float)moveSpeedField.GetValue(moveProvider);
                }
                else
                {
                    // fallback: try common alternative property name
                    var propAlt = t.GetProperty("m_MoveSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propAlt != null && propAlt.PropertyType == typeof(float))
                        baseMoveSpeed = (float)propAlt.GetValue(moveProvider);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"ComfortSettingsMenu: failed to read move speed via reflection: {ex.Message}");
                baseMoveSpeed = 1f;
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
        if (playerCamera == null) return;

        Vector3 forward = playerCamera.transform.forward;
        Vector3 target = playerCamera.transform.position + forward.normalized * openDistance + Vector3.up * verticalOffset;

        settingsPanel.transform.position = target;

        Vector3 lookDir = playerCamera.transform.position - settingsPanel.transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
            settingsPanel.transform.rotation = Quaternion.LookRotation(lookDir);
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
            if (moveSpeedProperty != null && moveSpeedProperty.PropertyType == typeof(float))
            {
                moveSpeedProperty.SetValue(moveProvider, newSpeed);
                return;
            }

            if (moveSpeedField != null && moveSpeedField.FieldType == typeof(float))
            {
                moveSpeedField.SetValue(moveProvider, newSpeed);
                return;
            }

            // fallback: try setting a non-public property/field
            var propAlt = moveProvider.GetType().GetProperty("m_MoveSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (propAlt != null && propAlt.PropertyType == typeof(float))
            {
                propAlt.SetValue(moveProvider, newSpeed);
                return;
            }

            Debug.LogWarning("ComfortSettingsMenu: moveSpeed property/field not found on the assigned moveProvider. Motion sensitivity won't change runtime speed.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"ComfortSettingsMenu: failed to set move speed via reflection: {ex.Message}");
        }
    }

    private void SetGazeEnabled(bool isEnabled)
    {
        if (gazeInteractor == null) return;

        // many gaze interactors are MonoBehaviours; enable/disable them
        gazeInteractor.enabled = isEnabled;

        // try to find a "reticle" field/property and toggle its GameObject if present
        var t = gazeInteractor.GetType();
        var reticleProp = t.GetProperty("reticle", BindingFlags.Public | BindingFlags.Instance);
        if (reticleProp != null)
        {
            var retObj = reticleProp.GetValue(gazeInteractor) as Component;
            if (retObj != null)
                retObj.gameObject.SetActive(isEnabled);
        }
        else
        {
            // try field
            var retField = t.GetField("reticle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (retField != null)
            {
                var retObj = retField.GetValue(gazeInteractor) as Component;
                if (retObj != null)
                    retObj.gameObject.SetActive(isEnabled);
            }
        }
    }
}
