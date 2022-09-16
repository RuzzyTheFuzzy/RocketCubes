using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text text;

    public void FuelUIUpdate(float fuel, float fuelMax)
    {
        rectTransform.anchorMax = new Vector2(fuel / fuelMax, rectTransform.anchorMax.y);
        text.text = "Fuel: " + fuel + "/" + fuelMax;
    }
}
