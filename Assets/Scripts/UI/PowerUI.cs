using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public void PowerUIUpdate(float power, float powerMax)
    {
        rectTransform.anchorMax = new Vector2(power / powerMax, rectTransform.anchorMax.y);
    }

}
