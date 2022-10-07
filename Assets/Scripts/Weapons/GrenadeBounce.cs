using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBounce : MonoBehaviour
{
    [SerializeField] private AudioSource bounce;

    private void OnCollisionEnter(Collision other)
    {
        // Audio for bounce
        bounce.Play();
    }
}
