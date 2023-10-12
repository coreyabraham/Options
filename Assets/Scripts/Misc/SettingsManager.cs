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
    public TMP_Dropdown resDropdown;
    public Settings settings;
    
    private GameObject currentFrame;
    private GameObject previousFrame;

    #region CurrentVariables

    private float currentMasterVolume;
    private float currentSoundVolume;
    private float currentMusicVolume;

    private int currentResolution;
    private bool currentFullscreen;
    private float currentHudScale;

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

    void Start()
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
                settings.resolution = currentResolution;
            }
        }

        if (resDropdown.options.Count > 0)
            resDropdown.ClearOptions();

        resDropdown.value = settings.resolution;

        resDropdown.AddOptions(options);
        resDropdown.RefreshShownValue();

        //resDropdown.onValueChanged.AddListener();
    }
}
