using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float fuel;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxThrowForce;
    [SerializeField] private Vector3 throwAngle = Vector3.forward;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float controllerMultiplier = 1f;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject cameraFollow;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private GameObject grenade;
    [SerializeField] private LayerMask layerMask;
    private float throwForce = 0;
    private float topClamp = 89f;
    private float bottomClamp = -89f;
    private float threshold = 0.01f;
    private GroundCheck groundCheck;
    private bool grounded;
    private GameObject controller;
    private Rigidbody rigidBody;
    private PlayerInputController playerInputController;
    private PlayerInput playerInput;
    private bool isCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "Keyboard&Mouse";
        }
    }

    private void Awake()
    {
        controller = transform.parent.gameObject;
        playerInputController = controller.GetComponent<PlayerInputController>();
        playerInput = controller.GetComponent<PlayerInput>();
        rigidBody = ball.GetComponent<Rigidbody>();
        groundCheck = ball.GetComponent<GroundCheck>();
    }

    private void Update()
    {
        CameraMove();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    private void OnEnable()
    {
        cinemachineCamera.enabled = true;
        cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
    }

    private void OnDisable()
    {
        cinemachineCamera.enabled = false;
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        ThrowGrenade();
        grounded = groundCheck.isGrounded;
    }

    private void CameraMove()
    {
        cameraFollow.transform.position = ball.transform.position;
    }

    private void CameraRotation()
    {
        // if there is an input
        if (playerInputController.look.sqrMagnitude >= threshold)
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

    private void Move()
    {
        if (playerInputController.move.sqrMagnitude >= threshold)
        {
            // Make Matrix with only cameras y axis
            Vector3 cameraRotation = new Vector3(0f, cameraFollow.transform.rotation.eulerAngles.y, 0f);
            Vector3 force = new Vector3(playerInputController.move.x, 0f, playerInputController.move.y);
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(cameraFollow.transform.position, Quaternion.Euler(cameraRotation), cameraFollow.transform.lossyScale);
            force = matrix4X4.MultiplyVector(force);
            rigidBody.AddForce(force * speed);
        }
    }

    private void Jump()
    {
        if (playerInputController.jump && grounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        playerInputController.jump = false;
    }

    private void ThrowGrenade()
    {
        if (playerInputController.grenade)
        {
            throwForce = Mathf.Min(throwForce + 0.1f, maxThrowForce);
            Debug.Log(throwForce);
        }
        else if (throwForce > 0)
        {
            Vector3 velocity = rigidBody.velocity;
            Vector3 spawnPoint = ball.transform.position;
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(spawnPoint, cameraFollow.transform.rotation, ball.transform.lossyScale);
            Vector3 direction = matrix4X4.MultiplyVector(throwAngle);
            float distance = ball.transform.lossyScale.x / 2;
            if (!Physics.Raycast(new Ray(spawnPoint, direction), distance))
            {
                GameObject newGrenade = Instantiate(grenade, spawnPoint + (direction * distance), cameraFollow.transform.rotation);
                Rigidbody[] bodies = newGrenade.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody body in bodies)
                {
                    body.velocity = velocity;
                    body.AddForce(direction * throwForce, ForceMode.Impulse);
                }
                Debug.LogError("You just threw the nade");
            }
            throwForce = 0;
            playerInputController.grenade = false;
        }
    }

}
