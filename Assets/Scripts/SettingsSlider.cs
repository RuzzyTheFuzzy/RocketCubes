using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsSlider : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] private string standardText;
    [SerializeField] private SettingsValue settingsValue;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        text.text = standardText + Mathf.RoundToInt(settingsValue.value);
        slider.value = settingsValue.value;
    }

    public void OnChange(float value)
    {
        text.text = standardText + Mathf.RoundToInt(value);
    }
}
