using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;

    private void sliderUpdated(float newValue)
    {
        inputField.text = newValue.ToString() + "%";
    }

    private void fieldUpdated(string newValue)
    {
        inputField.text = newValue + "%";
        float.TryParse(newValue, out float result);

        if (result > slider.maxValue)
            result = slider.maxValue;

        if (result < slider.minValue)
            result = slider.minValue;

        slider.value = result;
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(sliderUpdated);
        inputField.onEndEdit.AddListener(fieldUpdated);
    }
}
