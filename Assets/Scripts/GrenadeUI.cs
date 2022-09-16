using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeUI : MonoBehaviour
{

    [SerializeField] private Image[] grenadeImages = new Image[2];

    public void GrenadeUIUpdate(int grenades)
    {

        for (int i = 2; i > grenades; i--)
        {
            grenadeImages[i - 1].enabled = false;
        }

    }

}
