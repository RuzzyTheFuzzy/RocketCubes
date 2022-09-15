using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private float scaleDivider = 4f;
    private bool grounded;

    public bool isGrounded
    {
        get
        {
            return grounded;
        }
    }


    private void OnCollisionStay(Collision other)
    {
        grounded = false;
        ContactPoint[] contacts = new ContactPoint[other.contactCount];
        other.GetContacts(contacts);
        foreach (ContactPoint contactPoint in contacts)
        {
            float yDistance = transform.position.y - contactPoint.point.y;
            float jumpThreshold = transform.lossyScale.y / scaleDivider;
            if (yDistance >= jumpThreshold)
            {
                grounded = true;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        grounded = false;
    }

}
