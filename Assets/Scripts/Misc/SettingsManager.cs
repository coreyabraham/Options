using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [System.Serializable]
    public class Target
    {
        public GameObject Frame;
        public Button Button;
    }

    [Header("Frames and Buttons")]
    public List<Target> targets = new List<Target>();

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

    #region CurrentVariables

    private int currentMasterVolume;
    private int currentSoundVolume;
    private int currentMusicVolume;

    private int currentResolution;
    private bool currentFullscreen;
    private float currentHudScale;

    #endregion

    #region Menu Controls
    
    private void masterVolumeChanged(float value)
    {
        currentMasterVolume = (int)Math.Round(value);
        Debug.Log(currentMasterVolume);
    }

    private void soundVolumeChanged(float value)
    {
        currentSoundVolume = (int)Math.Round(value);
        Debug.Log(currentSoundVolume);
    }

    private void musicVolumeChanged(float value)
    {
        currentMusicVolume = (int)Math.Round(value);
        Debug.Log(value);
    }

    private void fullscreenChanged(bool fullscreen)
    {
        Debug.Log(fullscreen);
    }

    private void resolutionChanged(int resolution)
    {
        currentResolution = resolution;
        Debug.Log(resolution);
    }

    private void hudScaleChanged(float scale)
    {
        Debug.Log(scale);
    }

    #endregion

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

    private void PostInitSetup()
    {
        masterVolumeSlider.slider.value = settings.baseSettings.masterVolume;
        soundVolumeSlider.slider.value = settings.baseSettings.soundVolume;
        musicVolumeSlider.slider.value = settings.baseSettings.musicVolume;

        currentMasterVolume = settings.baseSettings.masterVolume;
        currentSoundVolume = settings.baseSettings.soundVolume;
        currentMusicVolume = settings.baseSettings.musicVolume;

        masterVolumeSlider.slider.onValueChanged.AddListener(masterVolumeChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(soundVolumeChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(musicVolumeChanged);

        fullscreenCheckbox.toggle.onValueChanged.AddListener(fullscreenChanged);
        resolutionDropdown.dropdown.onValueChanged.AddListener(resolutionChanged);
        hudScaleSlider.slider.onValueChanged.AddListener(hudScaleChanged);
    }

    public void Initilize()
    {
        currentFrame = targets[0].Frame;
        title.text = "Settings - " + currentFrame.name;

        if (!currentFrame.activeSelf)
            currentFrame.SetActive(true);

        foreach (Target i in targets)
            i.Button.onClick.AddListener(delegate { ToggleMenu(i); });

        List<string> options = new List<string>();
        settings.resolutions.Reverse();

        for (int i = 0; i < settings.resolutions.Length; i++)
        {
            string option = settings.resolutions[i].width + "x" + settings.resolutions[i].height;
            options.Add(option);

            bool matchingResolutions = settings.resolutions[i].width == Screen.width
                && settings.resolutions[i].height == Screen.height;

            if (matchingResolutions)
            {
                currentResolution = i;
                settings.baseSettings.resolution = currentResolution;
            }
        }

        if (resolutionDropdown.dropdown.options.Count > 0)
            resolutionDropdown.dropdown.ClearOptions();

        resolutionDropdown.dropdown.value = settings.baseSettings.resolution;

        resolutionDropdown.dropdown.AddOptions(options);
        resolutionDropdown.dropdown.RefreshShownValue();

        PostInitSetup();
    }
}
