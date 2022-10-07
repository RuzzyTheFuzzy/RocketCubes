using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsSlider : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] private OptionsValue optionsValue;
    // Scriptable object to make script more adjustable
    private string standardText;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        // More visiable in the editor this way.
        standardText = text.text;
    }

    private void Start()
    {
        text.text = standardText + Mathf.RoundToInt(optionsValue.value);
        slider.value = optionsValue.value;
    }

    public void OnChange(float value)
    {
        text.text = standardText + Mathf.RoundToInt(value);
    }
}
