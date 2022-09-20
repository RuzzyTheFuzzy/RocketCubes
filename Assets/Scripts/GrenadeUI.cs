using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeUI : MonoBehaviour
{

    [SerializeField] private Image[] grenadeImages = new Image[2];

    public void GrenadeUIUpdate(int grenades)
    {

        foreach (Image image in grenadeImages)
        {
            image.enabled = false;
        }

        for (int i = 0; i < grenades; i++)
        {
            grenadeImages[i].enabled = true;
        }

    }

}
