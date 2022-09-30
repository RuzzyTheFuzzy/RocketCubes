using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] private Image[] heartImages = new Image[3];

    public void HpUIUpdate(int HP)
    {

        foreach (Image image in heartImages)
        {
            image.enabled = false;
        }

        for (int i = 0; i < HP; i++)
        {
            heartImages[i].enabled = true;
        }

    }
}
