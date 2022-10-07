using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Rigidbody handle;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Rigidbody pin;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float radius;
    [SerializeField] private float power;
    private float explodeTimer;
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
        // Makes it look cool when thrown
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
    }

    private void Explode()
    {
        GameObject newExplosion = Instantiate(explosion, body.position, Quaternion.Euler(Vector3.zero));
        newExplosion.GetComponent<Explosion>().StartExplosion(power, radius);
        Destroy(gameObject);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(body.position, radius);
    }

}
