using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Rigidbody handle;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Rigidbody pin;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius;
    [SerializeField] private float power;
    private float explodeTimer;
    private float despawnTimer = 5f;
    private bool despawn = false;
    private bool timerStarted = false;
    public float ExplodeTimer
    {
        set
        {
            if (!timerStarted)
            {
                explodeTimer = value;
                timerStarted = true;
            }
        }
    }

    public Rigidbody Handle
    {
        get => handle;
    }
    public Rigidbody Body
    {
        get => body;
    }
    public Rigidbody Pin
    {
        get => pin;
    }

    private void Awake()
    {
        handle.AddRelativeForce(Vector3.forward * 5f);
        handle.AddRelativeTorque(Vector3.down * 5f);
        pin.AddRelativeForce(Vector3.back * 2f);
    }

    private void FixedUpdate()
    {
        if (timerStarted)
        {
            if (explodeTimer > 0)
            {
                explodeTimer -= Time.deltaTime;
            }
            else
            {
                Explode();
                timerStarted = false;
            }
        }
        // Remove the pins and handles and this script after despawnTimer seconds of Explode running
        if (despawn)
        {
            despawnTimer -= Time.deltaTime;
            if (despawnTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(body.position, radius, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody != null)
            {
                Rigidbody rigidBody = collider.attachedRigidbody;
                rigidBody.AddForce(Vector3.Normalize(rigidBody.position - body.position) * power
                // Mathf.Max to avoid dividing with 0 altough unlikely
                / Mathf.Max(Mathf.Sqrt(Vector3.Distance(rigidBody.position, body.position)), 1f),
                ForceMode.Impulse);
            }
        }
        Destroy(body.gameObject);
        despawn = true;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        // Avoid annoying errors when _body is gone but this script is still alive
        if (body != null)
        {
            Gizmos.DrawSphere(body.position, radius);
        }
    }

}
