using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{

    [SerializeField] TMP_Text text;

    public void TimerUIUpdate(float time)
    {
        text.text = Mathf.Ceil(time).ToString();
    }

}
