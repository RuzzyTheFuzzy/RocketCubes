using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrenadeUI : MonoBehaviour
{

    [SerializeField] private TMP_Text grenadeText;
    [SerializeField] private Image grenadeImage;
    [SerializeField] private Color antiColor;

    public void GrenadeUIUpdate(int grenades, bool anti)
    {

        grenadeText.text = "x " + grenades;

        if (anti)
        {
            grenadeImage.color = antiColor;
        }
        else
        {
            grenadeImage.color = Color.white;
        }
    }

}
