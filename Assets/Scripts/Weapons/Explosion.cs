using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private ParticleSystem particle;
    private float strength;
    private float explodeRadius;
    private bool explode = false;

    public void StartExplosion(float power, float radius)
    {
        strength = power;
        explodeRadius = radius;
        explode = true;
        particle.Play();
    }

    private void FixedUpdate()
    {
        if (explode)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.attachedRigidbody != null)
                {
                    Rigidbody rigidBody = collider.attachedRigidbody;
                    rigidBody.AddForce(Vector3.Normalize(rigidBody.position - transform.position) * strength);
                }
            }
        }
    }

    private void Update()
    {
        // FixedUpdate uses a different interval then Update, so deltatime only works correctly in update
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            explode = false;
            Destroy(gameObject);
        }
    }

}
