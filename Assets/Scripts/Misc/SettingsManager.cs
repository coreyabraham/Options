#pragma warning disable IDE0051 // Remove unused private members

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using TMPro;

public class SettingsManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class Target
    {
        public GameObject Frame;
        public ActionButtonManager Action;
    }

    [Serializable]
    public class Events
    {
        public string eventName;
        public UnityEvent acceptedEvent;
        public UnityEvent deniedEvent;
    }

    #region Unity Inspector

    [Header("Frames and Buttons")]
    public List<Target> targets = new List<Target>();
    public PromptManager promptFrame;

    [Header("Confirm / Deny")]
    public ActionButtonManager applyBtn;
    public ActionButtonManager revertBtn;

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
    public string[] mixerValues = new string[3];

    #endregion

    #region Miscellaneous Values

    private GameObject currentFrame;
    private GameObject previousFrame;

    private UnityEvent<bool, string> passthroughEvent;

    [Header("Miscellaneous")]
    public List<Events> applicableEvents = new();

    #endregion

    #region CurrentVariables

    private float currentMasterVolume;
    private float currentSoundVolume;
    private float currentMusicVolume;

    private int currentResolution;
    private bool currentFullscreen;
    private float currentHudScale;

    #endregion

    #region Base Buttons

    public void ApplySettings()
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

        Debug.Log(currentMusicVolume);

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

    public void RevertSettings()
    {
        Debug.LogWarning("Resetting Settings back to their defaults defined within the Settings Inspector!");

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

    public void ApplyDenied()
    {
        promptFrame.SetFrameVisibility(false);
    }

    public void RevertDenied()
    {
        promptFrame.SetFrameVisibility(false);
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

    #region Prompt Functions

    public void OnPromptFrame(string eventName)
    {
        promptFrame.HookButtons(eventName);
        promptFrame.SetEvent(passthroughEvent);
        promptFrame.SetFrameVisibility(true);
    }

    public void PromptResultReceived(bool result, string eventName)
    {
        promptFrame.SetFrameVisibility(false);

        foreach (Events i in applicableEvents)
        {
            if (i.eventName == eventName)
            {
                UnityEvent unityEvent;

                if (result)
                    unityEvent = i.acceptedEvent;
                else
                    unityEvent = i.deniedEvent;

                unityEvent.Invoke();
            }
        }
    }

    #endregion

    #region Event Functions

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Object Grabbed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Object Let Go");
    }

    #endregion

    #region Slider Controls

    private float AudioSliderCalculations(float value)
    {
        // original math equation: (float)Math.Log10(Math.Pow(10, value / 10)) * 10 - 100;
        return Mathf.Log10(value) * 20;
    }

    private void MasterSliderChanged(float value)
    {
        currentMasterVolume = value;
        gameMixer.SetFloat(mixerValues[0], AudioSliderCalculations(value));
    }

    private void SoundSliderChanged(float value)
    {
        currentSoundVolume = value;
        gameMixer.SetFloat(mixerValues[1], AudioSliderCalculations(value));
    }

    private void MusicSliderChanged(float value)
    {
        currentMusicVolume = value;
        gameMixer.SetFloat(mixerValues[2], AudioSliderCalculations(value));
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
        settings.baseSettings.masterVolume = Mathf.Clamp(
            settings.baseSettings.masterVolume, 
            masterVolumeSlider.slider.minValue, 
            masterVolumeSlider.slider.maxValue
            );

        settings.baseSettings.soundVolume = Mathf.Clamp(
            settings.baseSettings.soundVolume, 
            soundVolumeSlider.slider.minValue, 
            soundVolumeSlider.slider.maxValue
            );

        settings.baseSettings.musicVolume = Mathf.Clamp(
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
            settings.resolutions.Length - 1
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
        HudScaleSliderChanged(hudScaleSlider.slider.value);

        currentMasterVolume = settings.baseSettings.masterVolume;
        currentSoundVolume = settings.baseSettings.soundVolume;
        currentMusicVolume = settings.baseSettings.musicVolume;
        
        gameMixer.SetFloat(mixerValues[0], AudioSliderCalculations(currentMasterVolume));
        gameMixer.SetFloat(mixerValues[1], AudioSliderCalculations(currentSoundVolume));
        gameMixer.SetFloat(mixerValues[2], AudioSliderCalculations(currentMusicVolume));

        currentResolution = settings.baseSettings.resolution;
        currentFullscreen = settings.baseSettings.fullscreen;
        currentHudScale = settings.baseSettings.hudScale;

        masterVolumeSlider.slider.onValueChanged.AddListener(MasterSliderChanged);
        soundVolumeSlider.slider.onValueChanged.AddListener(SoundSliderChanged);
        musicVolumeSlider.slider.onValueChanged.AddListener(MusicSliderChanged);

        fullscreenCheckbox.toggle.onValueChanged.AddListener((bool value) => { currentFullscreen = value; });
        resolutionDropdown.dropdown.onValueChanged.AddListener((int value) => { currentResolution = value; });
        hudScaleSlider.slider.onValueChanged.AddListener(HudScaleSliderChanged);

        // currently hard-coded for the moment, this will be fixed in due time!
        applyBtn.button.onClick.AddListener(() => { OnPromptFrame("apply"); });
        revertBtn.button.onClick.AddListener(() => { OnPromptFrame("revert"); });
    }

    #endregion

    #region Invoke Controlled

    public void Initilize()
    {
        if (passthroughEvent == null)
        {
            passthroughEvent = new();
            passthroughEvent.AddListener(PromptResultReceived);
        }

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