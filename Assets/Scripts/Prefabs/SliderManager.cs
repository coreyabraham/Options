using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;
    public bool usePercentage;

    private void sliderUpdated(float newValue)
    {
        string endStr = "%";
        if (!usePercentage)
            endStr = string.Empty;

        inputField.text = newValue.ToString() + endStr;
    }

    private void fieldUpdated(string newValue)
    {
        string endStr = "%";
        if (!usePercentage)
            endStr = string.Empty;

        inputField.text = newValue + endStr;
        float.TryParse(newValue, out float result);

        if (result > slider.maxValue)
            result = slider.maxValue;

        if (result < slider.minValue)
            result = slider.minValue;

        slider.value = result;
    }

    private void Start()
    {
        string endStr = "%";
        if (!usePercentage)
            endStr = string.Empty;

        inputField.text = slider.minValue.ToString() + endStr;
        
        slider.onValueChanged.AddListener(sliderUpdated);
        inputField.onEndEdit.AddListener(fieldUpdated);
    }
}
