using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{

    [SerializeField] TMP_Text text;

    public void TimerUIUpdate(float time)
    {
        // Ceil so that the instance 0 is shown, the turn is over
        text.text = Mathf.Ceil(time).ToString();
    }

}
