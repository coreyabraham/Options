using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;

    [Header("Video")]
    public int resolution;
    public bool fullscreen;
    public float hudScale;

    [HideInInspector] public Resolution[] resolutions;

    // DEFAULTS //
    [HideInInspector] public float defaultMasterVolume;
    [HideInInspector] public float defaultSoundVolume;
    [HideInInspector] public float defaultMusicVolume;

    [HideInInspector] public int defaultResolution;
    [HideInInspector] public bool defaultFullscreen;
    [HideInInspector] public float defaultHudScale;

    private void Start()
    {
        resolutions = Screen.resolutions;

        defaultMasterVolume = masterVolume;
        defaultSoundVolume = soundVolume;
        defaultMusicVolume = musicVolume;

        defaultResolution = resolution;
        defaultFullscreen = fullscreen;
        defaultHudScale = hudScale;
    }
}
