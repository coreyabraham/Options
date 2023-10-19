using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public Slider slider;
    private AudioSource source;

    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("Mouse Down on Object!");
    }

    public void MediaVolumeAdjusted(float value)
    {
        source.volume = value / slider.maxValue;
    }

    private void Start()
    {
        AudioSource potentialSource = GetComponent<AudioSource>();

        if (potentialSource == null)
        {
            Debug.LogWarning("Could not find AudioSource within GameObject: " + name + "!");
            return;
        }

        source = potentialSource;
        source.volume = float.MinValue;

        if (slider == null)
        {
            Debug.LogWarning("Was not provided 'Slider' UI object within: " + name + "!");
            return;
        }

        slider.onValueChanged.AddListener(MediaVolumeAdjusted);
    }
}
