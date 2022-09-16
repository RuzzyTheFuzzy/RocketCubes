using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private GameObject grenade;
    [SerializeField] private float handleMultiplier = 0.5f;
    [SerializeField] private float pinMultiplier = 0f;
    [SerializeField] private float explodeTimer = 10f;

    public bool GrenadeToss(Rigidbody rigidBody, GameObject ball, GameObject cameraFollow, Vector3 throwAngle, float throwForce)
    {
        bool thrown = false;
        Vector3 velocity = rigidBody.velocity;
        Vector3 spawnPoint = ball.transform.position;
        Quaternion rotation = cameraFollow.transform.rotation;
        rotation.SetLookRotation(Vector3.back);
        Matrix4x4 matrix4X4 = Matrix4x4.TRS(spawnPoint, cameraFollow.transform.rotation, ball.transform.lossyScale);
        Vector3 direction = matrix4X4.MultiplyVector(throwAngle);
        float distance = ball.transform.lossyScale.x / 2;
        if (!Physics.Raycast(new Ray(spawnPoint, direction), distance))
        {
            thrown = true;
            Grenade newGrenade = Instantiate(grenade, spawnPoint + (direction * distance), rotation, transform).GetComponent<Grenade>();
            newGrenade.body.AddForce(throwForce * direction, ForceMode.Impulse);
            newGrenade.handle.AddForce(throwForce * direction * handleMultiplier, ForceMode.Impulse);
            newGrenade.pin.AddForce(throwForce * direction * pinMultiplier, ForceMode.Impulse);
            newGrenade.explodeTimer = explodeTimer;
        }
        return thrown;
    }

}
