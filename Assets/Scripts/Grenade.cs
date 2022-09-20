using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Rigidbody _handle;
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Rigidbody _pin;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius;
    public float power;
    private float _explodeTimer;
    private float despawnTimer = 5f;
    private bool despawn = false;
    private bool timerStarted = false;
    public float explodeTimer
    {
        set
        {
            if (!timerStarted)
            {
                _explodeTimer = value;
                timerStarted = true;
            }
        }
    }

    public Rigidbody handle
    {
        get => _handle;
    }
    public Rigidbody body
    {
        get => _body;
    }
    public Rigidbody pin
    {
        get => _pin;
    }

    private void Awake()
    {
        _handle.AddRelativeForce(Vector3.forward * 5f);
        _handle.AddRelativeTorque(Vector3.down * 5f);
        _pin.AddRelativeForce(Vector3.back * 2f);
    }

    private void FixedUpdate()
    {
        if (timerStarted)
        {
            if (_explodeTimer > 0)
            {
                _explodeTimer -= Time.deltaTime;
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
                Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(_body.position, radius, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody != null)
            {
                Rigidbody rigidBody = collider.attachedRigidbody;
                // Mathf.Max to avoid dividing with 0 altough unlikely
                rigidBody.AddForce(Vector3.Normalize(rigidBody.position - _body.position) * power / Mathf.Max(Mathf.Sqrt(Vector3.Distance(rigidBody.position, _body.position)), 1f), ForceMode.Impulse);
            }
        }
        Destroy(_body.gameObject);
        despawn = true;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        // Avoid annoying errors when _body is gone but this script still being alive
        if (_body != null)
            Gizmos.DrawSphere(_body.position, radius);
    }

}
