using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Serializable]
    public class Target
    {
        public GameObject Frame;
        public ActionButtonManager Action;
    }

    #region Unity Inspector

    [Header("Frames and Buttons")]
    public List<Target> targets = new List<Target>();

    [Header("Confirm / Deny")]
    public Button applyBtn;
    public Button revertBtn;

    [Header("Setup")]
    public TMP_Text title;
    public Settings settings;
    public RectTransform safeZone;

    [Header("Values - Audio")]
    public SliderManager masterVolumeSlider;
    public SliderManager soundVolumeSlider;
    public SliderManager musicVolumeSlider;

    [Header("Values - Video")]
    public CheckboxManager fullscreenCheckbox;
    public DropdownManager resolutionDropdown;
    public SliderManager hudScaleSlider;

    [Header("Audio Mixer Groups")]
    public AudioMixer gameMixer;

    #endregion

    #region Miscellaneous Values

    private GameObject currentFrame;
    private GameObject previousFrame;

    #endregion

    #region CurrentVariables

    private int currentMasterVolume;
    private int currentSoundVolume;
    private int currentMusicVolume;

    private int currentResolution;
    private bool currentFullscreen;
    private float currentHudScale;

    #endregion

    #region Base Buttons

    private void ApplySettings()
    {
        #region Audio Adjustments

        #endregion

        #region Video Adjustments

#if UNITY_EDITOR
        Debug.LogWarning("Current runtime is running under the Unity Editor, some video settings may be ignored!");
#endif
        Resolution targetResolution = settings.resolutions[currentResolution];
        Screen.SetResolution(targetResolution.width, targetResolution.height, currentFullscreen);

        #endregion

        #region Settings Finalizing

        BaseSettings newerSettings = new()
        {
            masterVolume = currentMasterVolume,
            soundVolume = currentSoundVolume,
            musicVolume = currentMusicVolume,

            resolution = currentResolution,
            fullscreen = currentFullscreen,
            hudScale = currentHudScale
        };

        settings.ApplyChanges(newerSettings);

        #endregion
    }

    private void RevertSettings()
    {
        #region Currents to Defaults

        currentMasterVolume = settings.defaultMasterVolume;
        currentSoundVolume = settings.defaultSoundVolume;
        currentMusicVolume = settings.defaultMusicVolume;

        currentResolution = settings.defaultResolution;
        currentFullscreen = settings.defaultFullscreen;
        currentHudScale = settings.defaultHudScale;

        #endregion

        #region UI Objects to Currents

        masterVolumeSlider.slider.value = currentMasterVolume;
        soundVolumeSlider.slider.value = currentSoundVolume;
        musicVolumeSlider.slider.value = currentMusicVolume;

        resolutionDropdown.dropdown.value = currentResolution;
        fullscreenCheckbox.toggle.isOn = currentFullscreen;
        hudScaleSlider.slider.value = currentHudScale;

        #endregion
    }

    private void ToggleMenu(Target Data)
    {
        if (currentFrame == Data.Frame)
            return;

        Data.Frame.SetActive(!Data.Frame.activeSelf);

        previousFrame = currentFrame;
        currentFrame = Data.Frame;

        if (previousFrame.activeSelf)
            previousFrame.SetActive(false);

        title.text = "Settings - " + currentFrame.name;
    }

    #endregion

    #region Slider Controls

    private float AudioSliderCalculations(float value)
    {
        return (float)Math.Log10(Math.Pow(10, value / 10)) * 10 - 100;
    }

    private void MasterSliderChanged(float value)
    {
        currentMasterVolume = (int)Math.Round(value);
        gameMixer.SetFloat("ExposedMasterVolume", AudioSliderCalculations(value));
    }

    private void SoundSliderChanged(float value)
    {
        currentSoundVolume = (int)Math.Round(value);
        gameMixer.SetFloat("ExposedSoundVolume", AudioSliderCalculations(value));
    }

    private void MusicSliderChanged(float value)
    {
        currentMusicVolume = (int)Math.Round(value);
        gameMixer.SetFloat("ExposedMusicVolume", AudioSliderCalculations(value));
    }

    private void HudScaleSliderChanged(float value)
    {
        currentHudScale = (float)Math.Round(value, 2);
        float result = Mathf.Lerp(0f, 100f, currentHudScale);
        safeZone.sizeDelta = new Vector2(-result, -result);
    }

    #endregion

    #region Private Functions

    private void SetupResolutions()
    {
        List<string> options = new();
        settings.resolutions.Reverse();

        for (int i = 0; i < settings.resolutions.Length; i++)
        {
            string option = settings.resolutions[i].width + "x" + settings.resolutions[i].height 
                + " - @" + settings.resolutions[i].refreshRate + "hz";
            options.Add(option);
        }

        if (resolutionDropdown.dropdown.options.Count > 0)
            resolutionDropdown.dropdown.ClearOptions();

        resolutionDropdown.dropdown.AddOptions(options);
        resolutionDropdown.dropdown.RefreshShownValue();

        resolutionDropdown.dropdown.value = settings.baseSettings.resolution;
    }

    private void ValidateData()
    {
        settings.baseSettings.masterVolume = (int)Mathf.Clamp(
            settings.baseSettings.masterVolume, 
            masterVolumeSlider.slider.minValue, 
            masterVolumeSlider.slider.maxValue
            );

        settings.baseSettings.soundVolume = (int)Mathf.Clamp(
            settings.baseSettings.soundVolume, 
            soundVolumeSlider.slider.minValue, 
            soundVolumeSlider.slider.maxValue
            );

        settings.baseSettings.musicVolume = (int)Mathf.Clamp(
            settings.baseSettings.musicVolume, 
            musicVolumeSlider.slider.minValue, 
            musicVolumeSlider.slider.maxValue
            );


        settings.baseSettings.hudScale = Mathf.Clamp(
            settings.baseSettings.hudScale, 
            hudScaleSlider.slider.minValue, 
            hudScaleSlider.slider.maxValue
            );

        settings.baseSettings.resolution = Mathf.Clamp(
            settings.baseSettings.resolution,
            0,
            settings.resolutions.Length
            );
    }

    private void PostInitSetup()
    {
        masterVolumeSlider.slider.value = settings.baseSettings.masterVolume;
        soundVolumeSlider.slider.value = settings.baseSettings.soundVolume;
        musicVolumeSlider.slider.value = settings.baseSettings.musicVolume;

        masterVolumeSlider.inputField.text = settings.baseSettings.masterVolume.ToString() + "%";
        soundVolumeSlider.inputField.text = settings.baseSettings.soundVolume.ToString() + "%";
        musicVolumeSlider.inputField.text = settings.baseSettings.musicVolume.ToString() + "%";

        fullscreenCheckbox.toggle.isOn = settings.baseSettings.fullscreen;
        hudScaleSlider.slider.value = settings.baseSettings.hudScale;
        hudScaleSlider.inputField.text = settings.baseSettings.hudScale.ToString() + "%";

        currentMasterVolume = settings.baseSettings.masterVolume;
        currentSoundVolume = settings.baseSettings.soundVolume;
        currentMusicVolume = settings.baseSettings.musicVolume;
        
        gameMixer.SetFloat("ExposedMasterVolume", AudioSliderCalculations(currentMasterVolume));
        gameMixer.SetFloat("ExposedSoundVolume", AudioSliderCalculations(currentSoundVolume));
        gameMixer.SetFloat("ExposedMusicVolume", AudioSliderCalculations(currentMusicVolume));

        currentResolution = settings.baseSettings.resolution;
        currentFullscreen = settings.baseSettings.fullscreen;
        currentHudScale = settings.baseSettings.hudScale;

        masterVolumeSlider.slider.onValueChanged.AddListener(MasterSliderChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(SoundSliderChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(MusicSliderChanged);

        fullscreenCheckbox.toggle.onValueChanged.AddListener((bool value) => { currentFullscreen = value; });
        resolutionDropdown.dropdown.onValueChanged.AddListener((int value) => { currentResolution = value; });
        hudScaleSlider.slider.onValueChanged.AddListener(HudScaleSliderChanged);

        applyBtn.onClick.AddListener(ApplySettings);
        revertBtn.onClick.AddListener(RevertSettings);
    }

    #endregion

    #region Invoke Controlled

    public void Initilize()
    {
        currentFrame = targets[0].Frame;
        title.text = "Settings - " + currentFrame.name;

        foreach (Target target in targets)
        {
            if (target.Frame == currentFrame)
            {
                if (!target.Frame.activeSelf)
                    target.Frame.SetActive(true);

                continue;
            }

            target.Frame.SetActive(false);
        }

        foreach (Target i in targets)
            i.Action.button.onClick.AddListener(delegate { ToggleMenu(i); });

        SetupResolutions();
        ValidateData();
        PostInitSetup();
    }

    #endregion
}
