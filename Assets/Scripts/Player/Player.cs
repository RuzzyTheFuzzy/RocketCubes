using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public float fuel;
    public float MaxFuel { get; private set; }
    public int grenades;
    public int Health { get; private set; } = 3;
    public int id;
    public float speed;
    [SerializeField] private float jumpForce;
    public Transform Ball { get; private set; }
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject cameraFollow;
    private LineRenderer line;
    public bool Anti { get; private set; }
    private float throwForce = 0;
    private float topClamp = 89f;
    private float bottomClamp = -89f;
    private GroundCheck groundCheck;
    private Rigidbody rigidBody;

    private PlayerInputController playerInputController
    {
        get
        {
            return GameManager.PlayerInputController;
        }
    }

    public bool grounded
    {
        get
        {
            return groundCheck.IsGrounded;
        }
    }

    public float power
    {
        get
        {
            return throwForce;
        }
    }

    private void Awake()
    {
        rigidBody = GetComponentInChildren<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        Ball = rigidBody.transform;
        MaxFuel = GameManager.PlayerManager.MaxFuel;
    }

    public void CameraMove()
    {
        cameraFollow.transform.position = Ball.position;
    }

    public void CameraRotation(float rotationSpeed, float controllerMultiplier)
    {
        // If there is an input
        if (playerInputController.look != Vector2.zero)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = playerInputController.IsCurrentDeviceMouse ? 1.0f : (Time.unscaledDeltaTime * controllerMultiplier);

            int xInverse = playerInputController.IsCurrentDeviceMouse ? -1 : 1;

            playerInputController.cameraTargetPitch.x += playerInputController.look.y * rotationSpeed * deltaTimeMultiplier * xInverse;
            playerInputController.cameraTargetPitch.y += playerInputController.look.x * rotationSpeed * deltaTimeMultiplier;

            // Clamp our pitch rotation
            playerInputController.cameraTargetPitch.x = ClampAngle(playerInputController.cameraTargetPitch.x, bottomClamp, topClamp);

            // Update Cinemachine camera target rotation
            cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
        {
            lfAngle += 360f;
        }
        if (lfAngle > 360f)
        {
            lfAngle -= 360f;
        }
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void Move()
    {
        if (playerInputController.move.sqrMagnitude > 0)
        {
            // X left-right, Z forward-backwards
            Vector3 direction = new Vector3(playerInputController.move.x, 0f, playerInputController.move.y);

            Vector3 cameraForward = cameraFollow.transform.forward;
            // We don't want any force upwards
            cameraForward.y = 0;

            direction = Quaternion.FromToRotation(Vector3.forward, cameraForward.normalized) * direction;



            float force = Mathf.Min(speed, fuel);
            rigidBody.AddForce(direction * force);
            fuel = Mathf.Max(fuel - force, 0);
        }
    }

    public void Jump()
    {
        if (playerInputController.jump && grounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        playerInputController.jump = false;
    }

    public void ThrowGrenade(float maxThrowForce, float throwAngle)
    {
        if (grenades > 0)
        {
            // Press Grenade Button
            if (playerInputController.grenade)
            {
                throwForce = Mathf.Min(throwForce + 0.1f, maxThrowForce);
                Vector3 direction = cameraFollow.transform.forward;
                direction.y += throwAngle;
                GameManager.WeaponManager.DrawProjection(cameraFollow.transform.position, throwForce, direction.normalized);
            }
            // Release it after having held it
            else if (throwForce > 0)
            {
                if (GameManager.WeaponManager.GrenadeToss(rigidBody, Ball, cameraFollow, throwAngle, throwForce, Anti))
                {
                    grenades--;
                }
                throwForce = 0;
                playerInputController.grenade = false;
            }
        }
        // Turn off the line when not tossing that grenade
        if (!playerInputController.grenade)
        {
            GameManager.WeaponManager.Line.enabled = false;
        }
    }

    public void Hit()
    {
        if (--Health <= 0)
        {
            GameManager.PlayerManager.Death(id);
        }
    }

    public void ChangeMaxFuel(float fuel)
    {
        MaxFuel += fuel;
    }

    public void SwapWeapons()
    {
        if (playerInputController.weaponSwap)
        {
            Anti = !Anti;
        }
        playerInputController.weaponSwap = false;
    }
}
