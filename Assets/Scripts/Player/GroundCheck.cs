using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private float scaleDivider = 4f;
    public bool IsGrounded { get; private set; } = false;

    // OnCollisionStay goes once per physics cycle for eatch collision
    private void OnCollisionStay(Collision other)
    {
        ContactPoint[] contacts = new ContactPoint[other.contactCount];
        other.GetContacts(contacts);
        foreach (ContactPoint contactPoint in contacts)
        {
            // If the angle from the point is about 45, you can jump
            float yDistance = transform.position.y - contactPoint.point.y;
            float jumpThreshold = transform.lossyScale.y / scaleDivider;
            if (yDistance >= jumpThreshold)
            {
                IsGrounded = true;
            }
        }
        // Even if no ground if found, we still assume character is grounded unless Exit says otherwise
    }

    private void OnCollisionExit(Collision other)
    {
        // If still grounded after exit, OnCollisionStay will correct
        IsGrounded = false;
    }
}
