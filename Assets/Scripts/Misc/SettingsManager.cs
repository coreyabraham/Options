using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Serializable]
    public class Target
    {
        public GameObject Frame;
        public Button Button;
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

    [Header("Values - Audio")]
    public SliderManager masterVolumeSlider;
    public SliderManager soundVolumeSlider;
    public SliderManager musicVolumeSlider;

    [Header("Values - Video")]
    public CheckboxManager fullscreenCheckbox;
    public DropdownManager resolutionDropdown;
    public SliderManager hudScaleSlider;

    private GameObject currentFrame;
    private GameObject previousFrame;

    #endregion

    #region CurrentVariables

    private int currentMasterVolume;
    private int currentSoundVolume;
    private int currentMusicVolume;

    private int currentResolution;
    private bool currentFullscreen;
    private int currentHudScale;

    #endregion

    #region Menu Controls

    private void masterVolumeChanged(float value)
    {
        currentMasterVolume = (int)Math.Round(value);
    }

    private void soundVolumeChanged(float value)
    {
        currentSoundVolume = (int)Math.Round(value);
    }

    private void musicVolumeChanged(float value)
    {
        currentMusicVolume = (int)Math.Round(value);
    }

    private void fullscreenChanged(bool fullscreen)
    {
        currentFullscreen = fullscreen;
    }

    private void resolutionChanged(int resolution)
    {
        currentResolution = resolution;
        resolutionDropdown.dropdown.RefreshShownValue(); // may not be necessary!
    }

    private void hudScaleChanged(float value)
    {
        Debug.Log(value);
        currentHudScale = (int)Math.Round(value);
    }

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
        // Add UI Scaling here!

#endregion

        #region Settings Finalizing

        BaseSettings newerSettings = new()
        {
            masterVolume = currentMasterVolume,
            soundVolume = currentSoundVolume,
            musicVolume = currentMusicVolume,

            resolution = currentResolution,
            fullscreen = currentFullscreen,
            hudScale = currentHudScale // why does this value keep resetting to "0" upon a file creation?
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

    #region Private Functions

    private void SetupResolutions()
    {
        List<string> options = new List<string>();
        settings.resolutions.Reverse();

        for (int i = 0; i < settings.resolutions.Length; i++)
        {
            string option = settings.resolutions[i].width + "x" + settings.resolutions[i].height;
            options.Add(option);
        }

        if (resolutionDropdown.dropdown.options.Count > 0)
            resolutionDropdown.dropdown.ClearOptions();

        resolutionDropdown.dropdown.AddOptions(options);
        resolutionDropdown.dropdown.RefreshShownValue();

        resolutionDropdown.dropdown.value = settings.baseSettings.resolution;
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

        masterVolumeSlider.slider.onValueChanged.AddListener(masterVolumeChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(soundVolumeChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(musicVolumeChanged);

        fullscreenCheckbox.toggle.onValueChanged.AddListener(fullscreenChanged);
        resolutionDropdown.dropdown.onValueChanged.AddListener(resolutionChanged);
        hudScaleSlider.slider.onValueChanged.AddListener(hudScaleChanged);

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
            i.Button.onClick.AddListener(delegate { ToggleMenu(i); });

        SetupResolutions();
        PostInitSetup();
    }

    #endregion
}
