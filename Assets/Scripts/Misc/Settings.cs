using UnityEngine;
using UnityEngine.Events;

using System.IO;
using System.IO.Enumeration;

[System.Serializable]
public class BaseSettings
{
    [Header("Audio")] // 0 - 100
    public int masterVolume;
    public int soundVolume;
    public int musicVolume;

    [Header("Video")]
    public int resolution; // 0+
    public bool fullscreen;
    public float hudScale; // 0.00 - 1.00
}

public class Settings : MonoBehaviour
{
    [SerializeField]
    public BaseSettings baseSettings;

    [Header("Miscellanous")]
    public string fileName = "SettingsData.json";
    private string filePath;
    public UnityEvent initalizeEvent;

    [HideInInspector] public Resolution[] resolutions;

    // DEFAULTS //
    [HideInInspector] public int defaultMasterVolume;
    [HideInInspector] public int defaultSoundVolume;
    [HideInInspector] public int defaultMusicVolume;

    [HideInInspector] public int defaultResolution;
    [HideInInspector] public bool defaultFullscreen;
    [HideInInspector] public float defaultHudScale;

    public void SetDefaults()
    {
        defaultMasterVolume = baseSettings.masterVolume;
        defaultSoundVolume = baseSettings.soundVolume;
        defaultMusicVolume = baseSettings.musicVolume;

        defaultResolution = baseSettings.resolution;
        defaultFullscreen = baseSettings.fullscreen;
        defaultHudScale = baseSettings.hudScale;
    }

    private void ManageData()
    {
        SetDefaults();

        #region Setup
        filePath = Application.persistentDataPath + "\\" + fileName;
        DataManager dataInst = new();
        #endregion

        #region Read Data
        if (!File.Exists(filePath))
        {
            Debug.Log("SettingsData.json file has been created in Directory: " + Application.persistentDataPath + "\\");
            dataInst.SaveAllData(baseSettings, filePath);
        }

        else
        {
            string jsonFileContents = File.ReadAllText(filePath);
            baseSettings = dataInst.GetData(jsonFileContents);
        }
        #endregion
    }

    void Start()
    {
        resolutions = Screen.resolutions;

        ManageData();

        if (initalizeEvent == null)
        {
            Debug.LogWarning("Initalize Event Empty, cannot invoke said event!");
            return;
        }

        initalizeEvent.Invoke();
    }
}
