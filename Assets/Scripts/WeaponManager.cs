using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private GameObject grenade;
    [SerializeField][Range(0f, 1f)] private float handleMultiplier = 0.5f;
    [SerializeField][Range(0f, 1f)] private float pinMultiplier = 0f;
    [SerializeField][Range(0.1f, 30f)] private float explodeTimer = 10f;
    [SerializeField][Range(10, 100)] private int linePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
    public LineRenderer line { get; private set; }
    private float mass;
    private LayerMask GrenadeCollisionMask;


    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        mass = grenade.GetComponent<Grenade>().body.mass;

        int grenadeLayer = grenade.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                GrenadeCollisionMask |= 1 << i; // magic
            }
        }
    }
    public bool GrenadeToss(Rigidbody rigidBody, Transform ball, GameObject cameraFollow, float throwAngle, float throwForce)
    {
        bool thrown = false;
        Vector3 velocity = rigidBody.velocity;
        Vector3 releasePosition = ball.position;
        Quaternion rotation = cameraFollow.transform.rotation;
        rotation.SetLookRotation(Vector3.back);
        Vector3 direction = cameraFollow.transform.forward;
        direction.y += throwAngle;
        direction.Normalize();
        float distance = ball.lossyScale.x / 2;
        if (!Physics.Raycast(new Ray(releasePosition, direction), distance))
        {
            thrown = true;
            Grenade newGrenade = Instantiate(grenade, releasePosition + (direction * distance), rotation, transform).GetComponent<Grenade>();
            newGrenade.body.AddForce(throwForce * direction, ForceMode.Impulse);
            newGrenade.handle.AddForce(throwForce * direction * handleMultiplier, ForceMode.Impulse);
            newGrenade.pin.AddForce(throwForce * direction * pinMultiplier, ForceMode.Impulse);
            newGrenade.explodeTimer = explodeTimer;
        }
        return thrown;
    }

    public void DrawProjection(Vector3 releasePosition, float throwForce, Vector3 direction)
    {
        line.enabled = true;
        line.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = releasePosition;
        Vector3 startVelocity = throwForce * direction / mass;
        int i = 0;
        line.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            line.SetPosition(i, point);

            Vector3 lastPosition = line.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                GrenadeCollisionMask))
            {
                line.SetPosition(i, hit.point);
                line.positionCount = i + 1;
                return;
            }
        }
    }

}
