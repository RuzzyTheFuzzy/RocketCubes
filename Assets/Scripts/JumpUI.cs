using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void JumpUIUpdate(bool jump)
    {
        if (!jump)
        {
            image.color = Color.black;

        }
        else
        {
            image.color = Color.blue;
        }
    }

}
