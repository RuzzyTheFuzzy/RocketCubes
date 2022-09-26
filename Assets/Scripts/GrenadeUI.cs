using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrenadeUI : MonoBehaviour
{

    [SerializeField] private TMP_Text grenadeText;

    public void GrenadeUIUpdate(int grenades)
    {

        grenadeText.text = "x " + grenades;

    }

}
