using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public float fuel;
    public int jumps;
    public int grenades;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject cameraFollow;
    [SerializeField] private GameObject ball;
    private float throwForce = 0;
    private float topClamp = 89f;
    private float bottomClamp = -89f;
    private GroundCheck groundCheck;
    private CharacterController controller;
    private GameObject manager;
    private Rigidbody rigidBody;
    private PlayerInput playerInput;
    private PlayerInputController playerInputController;
    private bool isCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "Keyboard&Mouse";
        }
    }

    public bool grounded
    {
        get
        {
            return groundCheck.isGrounded;
        }
    }

    private void Awake()
    {
        manager = transform.parent.gameObject;
        controller = manager.GetComponent<CharacterController>();
        playerInputController = manager.GetComponent<PlayerInputController>();
        playerInput = manager.GetComponent<PlayerInput>();
        rigidBody = ball.GetComponent<Rigidbody>();
        groundCheck = ball.GetComponent<GroundCheck>();
    }

    public void CameraMove()
    {
        cameraFollow.transform.position = ball.transform.position;
    }

    public void CameraRotation(float rotationSpeed, float controllerMultiplier)
    {
        // if there is an input
        if (playerInputController.look.sqrMagnitude > Vector2.zero.sqrMagnitude)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = isCurrentDeviceMouse ? 1.0f : (Time.deltaTime * controllerMultiplier);
            // Debug.Log(isCurrentDeviceMouse);

            playerInputController.cameraTargetPitch.x += playerInputController.look.y * rotationSpeed * deltaTimeMultiplier;
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
        if (playerInputController.move.sqrMagnitude > Vector2.zero.sqrMagnitude)
        {
            // Make Matrix with only cameras y axis
            Vector3 cameraRotation = new Vector3(0f, cameraFollow.transform.rotation.eulerAngles.y, 0f);
            Vector3 direction = new Vector3(playerInputController.move.x, 0f, playerInputController.move.y);
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(cameraFollow.transform.position, Quaternion.Euler(cameraRotation), cameraFollow.transform.lossyScale);
            direction = matrix4X4.MultiplyVector(direction);
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

    public void ThrowGrenade(float maxThrowForce, Vector3 throwAngle)
    {
        if (grenades > 0)
        {
            if (playerInputController.grenade)
            {
                throwForce = Mathf.Min(throwForce + 0.1f, maxThrowForce);
                Debug.Log(throwForce);
            }
            else if (throwForce > 0)
            {
                if (controller.weaponManager.GrenadeToss(rigidBody, ball, cameraFollow, throwAngle, throwForce))
                    grenades--;
                throwForce = 0;
                playerInputController.grenade = false;
            }
        }
    }
}
