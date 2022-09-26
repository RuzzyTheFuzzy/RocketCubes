using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public float fuel;
    public float maxFuel { get; private set; }
    public int jumps;
    public int grenades;
    public int health;
    public int id;
    public float speed;
    [SerializeField] private bool anti;
    [SerializeField] private float jumpForce;
    public Transform ball { get; private set; }
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject cameraFollow;
    private LineRenderer line;
    private float throwForce = 0;
    private float topClamp = 89f;
    private float bottomClamp = -89f;
    private GroundCheck groundCheck;
    private Rigidbody rigidBody;

    private PlayerInputController playerInputController
    {
        get
        {
            return GameManager.instance.playerInputController;
        }
    }

    private bool isCurrentDeviceMouse
    {
        get
        {
            return playerInputController.isCurrentDeviceMouse;
        }
    }

    public bool grounded
    {
        get
        {
            return groundCheck.isGrounded;
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
        ball = rigidBody.transform;
        maxFuel = GameManager.instance.playerManager.maxFuel;
    }

    public void CameraMove()
    {
        cameraFollow.transform.position = ball.position;
    }

    public void CameraRotation(float rotationSpeed, float controllerMultiplier)
    {
        // if there is an input
        if (playerInputController.look.sqrMagnitude > Vector2.zero.sqrMagnitude)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = isCurrentDeviceMouse ? 1.0f : (Time.deltaTime * controllerMultiplier);
            // Debug.Log(isCurrentDeviceMouse);
            int xInverse = isCurrentDeviceMouse ? -1 : 1;

            playerInputController.cameraTargetPitch.x += playerInputController.look.y * rotationSpeed * deltaTimeMultiplier * xInverse;
            playerInputController.cameraTargetPitch.y += playerInputController.look.x * rotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            playerInputController.cameraTargetPitch.x = ClampAngle(playerInputController.cameraTargetPitch.x, bottomClamp, topClamp);

            // Update Cinemachine camera target rotation
            cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
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
        if (playerInputController.jump && grounded && jumps > 0)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumps--;
        }
        playerInputController.jump = false;
    }

    public void ThrowGrenade(float maxThrowForce, float throwAngle)
    {
        if (grenades > 0)
        {
            if (playerInputController.grenade)
            {
                throwForce = Mathf.Min(throwForce + 0.1f, maxThrowForce);
                Vector3 direction = cameraFollow.transform.forward;
                direction.y += throwAngle;
                GameManager.instance.weaponManager.DrawProjection(cameraFollow.transform.position, throwForce, direction.normalized);
            }
            else if (throwForce > 0)
            {
                if (GameManager.instance.weaponManager.GrenadeToss(rigidBody, ball, cameraFollow, throwAngle, throwForce, anti))
                    grenades--;
                throwForce = 0;
                playerInputController.grenade = false;
            }
        }
        if (!playerInputController.grenade)
            GameManager.instance.weaponManager.line.enabled = false;
    }



    public void Hit()
    {
        if (--health <= 0)
        {
            GameManager.instance.playerManager.Death(id);
        }
    }

    public void ChangeMaxFuel(float fuel)
    {
        maxFuel += fuel;
    }
}
