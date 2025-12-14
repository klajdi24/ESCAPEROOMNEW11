using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(Canvas))]
public class ComfortSettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Button closeButton;
    public Slider brightnessSlider;
    public Slider motionSensitivitySlider;
    public Toggle gazeToggle;

    [Header("Audio Sliders")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Scene Lighting")]
    public Light[] sceneLights; 
    private float[] baseLightIntensities;

    [Header("Scene References")]
    public MonoBehaviour gazeInteractor;

    [Header("XR Locomotion")]
    public DynamicMoveProvider moveProvider;
    private float baseMoveSpeed = 1f;

    [Header("Placement")]
    public Camera playerCamera;
    public float openDistance = 1.5f;
    public float verticalOffset = -0.2f;

    private bool menuOpen = false;
    private InputAction toggleAction;

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
        // Store base light intensities
        if (sceneLights != null && sceneLights.Length > 0)
        {
            baseLightIntensities = new float[sceneLights.Length];
            for (int i = 0; i < sceneLights.Length; i++)
            {
                baseLightIntensities[i] = sceneLights[i].intensity;
            }
        }

        // Close button
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        // Brightness slider
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value = 0.5f;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            SetBrightness(brightnessSlider.value);
        }

        // Motion sensitivity slider
        if (motionSensitivitySlider != null)
        {
            motionSensitivitySlider.minValue = 0f;
            motionSensitivitySlider.maxValue = 1f;
            motionSensitivitySlider.value = 0.5f;
            motionSensitivitySlider.onValueChanged.AddListener(SetMotionSensitivity);
        }

        // Gaze toggle
        if (gazeToggle != null)
        {
            gazeToggle.onValueChanged.AddListener(SetGazeEnabled);

            if (gazeInteractor != null)
                gazeToggle.isOn = gazeInteractor.enabled;
        }

        // Audio sliders
        if (musicVolumeSlider != null && AudioManager.instance != null)
        {
            musicVolumeSlider.value = AudioManager.instance.musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        }

        if (sfxVolumeSlider != null && AudioManager.instance != null)
        {
            sfxVolumeSlider.value = AudioManager.instance.sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        }

        // Store base move speed
        if (moveProvider != null)
            baseMoveSpeed = moveProvider.moveSpeed;
    }

    private void Update()
    {
        if (toggleAction != null && toggleAction.WasPressedThisFrame())
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
        settingsPanel.SetActive(menuOpen);

        if (menuOpen)
            PlacePanelInFrontOfPlayer();
    }

    private void PlacePanelInFrontOfPlayer()
    {
        Vector3 target = playerCamera.transform.position +
                         playerCamera.transform.forward * openDistance +
                         Vector3.up * verticalOffset;

        settingsPanel.transform.position = target;
        settingsPanel.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);
    }

    public void CloseMenu()
    {
        menuOpen = false;
        settingsPanel.SetActive(false);
    }

    private void SetBrightness(float value)
    {
        if (sceneLights == null || baseLightIntensities == null) return;

        for (int i = 0; i < sceneLights.Length; i++)
        {
            if (sceneLights[i] != null)
            {
                sceneLights[i].intensity = Mathf.Lerp(0.2f, baseLightIntensities[i], value);
            }
        }
    }

    private void SetMotionSensitivity(float value)
    {
        if (moveProvider == null) return;

        moveProvider.moveSpeed = baseMoveSpeed * Mathf.Lerp(0.5f, 2f, value * value);
    }

    private void SetGazeEnabled(bool isEnabled)
    {
        if (gazeInteractor != null)
            gazeInteractor.enabled = isEnabled;

        Transform cursor = gazeInteractor.transform.Find("GazeCursor");
        if (cursor != null)
            cursor.gameObject.SetActive(isEnabled);
    }
}



